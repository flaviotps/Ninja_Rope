using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public GameObject shooterGuide;
    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] public DistanceJoint2D distanceJoint2D;
    [SerializeField] public Rigidbody2D rigidbody2D;
    [SerializeField] public BoxCollider2D boxCollider2D;
    [SerializeField] public int projectileForce = 1000;
    [SerializeField] public float jointHitLimiar = 0.1f;
    
    //PROJECTILE
    private GameObject _projectileGameObject;
    private Projectile _projectileScript;
    private Rigidbody2D _projectileRigidbody2D;
    
    private List<Vector2> _rayCastJointPoints = new List<Vector2>();

    void Start()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("projectilePrefab is null");
        }
        
        if (shooterGuide == null)
        {
            Debug.LogError("shooterGuide is null");
        }
        if (lineRenderer == null)
        {
            Debug.LogError("lineRenderer is null");
        }
        
        _projectileGameObject = Instantiate(projectilePrefab, shooterGuide.transform.position, shooterGuide.transform.rotation);
        _projectileGameObject.name = "Projectile";
        _projectileRigidbody2D = _projectileGameObject.GetComponent<Rigidbody2D>();
        _projectileScript = _projectileGameObject.GetComponent<Projectile>();
        _projectileScript.isHooked = false;
        //**@@@@@ ATTENTION @@@@@** 
        Physics2D.IgnoreCollision(boxCollider2D, _projectileGameObject.GetComponent<BoxCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        updateLine();
        handleJoint();
    }

    public void FireProjectile()
    {
        _rayCastJointPoints.Clear();
        _projectileScript.isHooked = false;
        _projectileGameObject.transform.position = transform.position;
        _projectileRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _projectileRigidbody2D.AddForce(_projectileGameObject.transform.up * projectileForce);
    }

    private void updateLine()
    {
        if (_projectileScript.isHooked)
        {
            //ALOCATE ENOUGH POSITIONS TO ALL SEGMENTS + LAST SEGMENT
            lineRenderer.positionCount = _rayCastJointPoints.Count + 1;
            //DRAW ALL LINES SEGMENTS
            for (var index = 0; index < _rayCastJointPoints.Count; index++)
            {
                lineRenderer.SetPosition(index, _rayCastJointPoints[index]);
            }

            //DRAW THE LAST LINE SEGMENT (CLOSEST HOOK JOINT TO PLAYER)
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, shooterGuide.transform.position);
        }
        else
        {
            lineRenderer.positionCount = 0;
            _rayCastJointPoints.Clear();
        }
    }


    private void handleJoint()
    {
        var isGrounded = Physics2D.IsTouchingLayers(boxCollider2D, LayerMask.GetMask("Wall"));
        if (_projectileScript.isHooked /*&& !isGrounded*/)
        {
            //ADDS THE INITIAL HOOK POSITION TO THE LIST
            if (_rayCastJointPoints.Count == 0)
            {
                var projectileHit = Physics2D.Linecast(shooterGuide.transform.position,
                    _projectileGameObject.transform.position,
                    LayerMask.GetMask("Projectile"));

                if (projectileHit.collider != null)
                {
                    _rayCastJointPoints.Add(projectileHit.point);
                    _projectileGameObject.transform.position = projectileHit.point;
                }
            }
            //ADDS HOOK JOINS WHEN THE ROPE COLLIDES WITH A WALL
            else if (_rayCastJointPoints.Count > 0)
            {
                var nextHit = Physics2D.Linecast(shooterGuide.transform.position,
                        _projectileGameObject.transform.position,
                        LayerMask.GetMask("Wall"));
                    if (nextHit.collider != null)
                    {
                        if (Vector2.Distance(nextHit.transform.position, _rayCastJointPoints[_rayCastJointPoints.Count - 1]) >
                            jointHitLimiar)
                        {
                            if (_rayCastJointPoints.FindAll(vector => vector == nextHit.point).Count == 0)
                            {
                                _rayCastJointPoints.Add(nextHit.point);
                                _projectileGameObject.transform.position = nextHit.point;
                            }
                        }
                    }

                    /*if (_rayCastJointPoints.Count >= 2)
                    {
                        var hitPrevious = Physics2D.Linecast(_rayCastJointPoints[_rayCastJointPoints.Count - 2],
                            shooterGuide.transform.position,
                            LayerMask.GetMask("Wall"));
                        if (hitPrevious.collider != null)
                        {
                           // _projectileGameObject.transform.position = _rayCastJointPoints[_rayCastJointPoints.Count - 2];
                           // _rayCastJointPoints.RemoveAt(_rayCastJointPoints.Count - 1);
                        }
                    }*/
            }

            distanceJoint2D.enabled = true;
            distanceJoint2D.connectedBody = _projectileRigidbody2D;
            distanceJoint2D.anchor = Vector2.zero;
        }
        else if(!_projectileScript.isHooked)
        {
            distanceJoint2D.enabled = false;
            distanceJoint2D.connectedBody = null;
            distanceJoint2D.anchor = Vector2.zero;
        }
    }
}
