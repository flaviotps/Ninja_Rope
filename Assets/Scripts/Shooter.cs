using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public class Shooter : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public GameObject shooterGuide;
    [SerializeField] public int projectileForce = 1000;
    [SerializeField] public int hookImpulse = 100;
    [SerializeField] public float jointHitLimiar = 0.1f;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public LayerMask projectileLayer;

    //PLAYER
    private DistanceJoint2D _distanceJoint2D;
    private LineRenderer _lineRenderer;
    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D _rigidbody2D;

    //PROJECTILE
    private GameObject _projectileGameObject;
    private Projectile _projectileScript;
    private Rigidbody2D _projectileRigidbody2D;
    private SpriteShapeRenderer _projectileSpriteShapeRenderer;
    
    private readonly List<Vector2> _rayCastJointPoints = new List<Vector2>();

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _projectileGameObject = Instantiate(projectilePrefab, shooterGuide.transform.position, shooterGuide.transform.rotation);
        _projectileRigidbody2D = _projectileGameObject.GetComponent<Rigidbody2D>();
        _projectileScript = _projectileGameObject.GetComponent<Projectile>();
        _projectileSpriteShapeRenderer = _projectileGameObject.GetComponent<SpriteShapeRenderer>();
        Physics2D.IgnoreCollision(_circleCollider2D, _projectileGameObject.GetComponent<CircleCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLine();
        HandleJoint();
    }

    public void FireProjectile()
    {
        if (_projectileScript.isHooked)
        {
            //RESET THE PROJECTILE 
            _rayCastJointPoints.Clear();
            _projectileScript.isHooked = false;
            _projectileSpriteShapeRenderer.enabled = false;
            _projectileRigidbody2D.simulated = false;
            _distanceJoint2D.enabled = false;
            _distanceJoint2D.connectedBody = null;
            _distanceJoint2D.anchor = Vector2.zero;
        }
        else
        {
            //SET THE PROJECTILE 
            _projectileSpriteShapeRenderer.enabled = true;
            _projectileRigidbody2D.simulated = true;
            _projectileGameObject.transform.position = shooterGuide.transform.position;
            _projectileRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            
            //SHOOTS THE PROJECTILE IN THE VELOCITY DIRECTION
            if (_rigidbody2D.velocity.magnitude == 0)
            {
                _projectileRigidbody2D.AddForce(_projectileGameObject.transform.up * projectileForce);
            }
            else
            {
                var force = NormalizeVelocityDirection(_rigidbody2D.velocity.normalized) * projectileForce;
                _projectileRigidbody2D.AddForce(force);
            }

        }
    }

    private void UpdateLine()
    {

        if (_rayCastJointPoints.Count == 0)
        {
            _lineRenderer.positionCount = 0;
        }
        else if (_projectileScript.isHooked)
        {
            //ALLOCATES ENOUGH POSITIONS TO ALL SEGMENTS + LAST SEGMENT
            _lineRenderer.positionCount = _rayCastJointPoints.Count + 1;
            //DRAW ALL LINES SEGMENTS
            for (var index = 0; index < _rayCastJointPoints.Count; index++)
            {
                _lineRenderer.SetPosition(index, _rayCastJointPoints[index]);
            }
            //DRAW THE LAST LINE SEGMENT (CLOSEST HOOK JOINT TO PLAYER)
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, shooterGuide.transform.position);
        }
        else
        {
            //CLEAR LINE WHEN NOT HOOKED
            _lineRenderer.positionCount = 0;
            _rayCastJointPoints.Clear();
        }
    }


    private void HandleJoint()
    {
        if (_projectileScript.isHooked)
        {
            //ADDS THE INITIAL HOOK POSITION TO THE LIST
            if (_rayCastJointPoints.Count == 0)
            {
                var projectileHit = Physics2D.Linecast(shooterGuide.transform.position,
                    _projectileGameObject.transform.position,projectileLayer);

                if (!ReferenceEquals(projectileHit.collider, null))
                {
                    _rayCastJointPoints.Add(projectileHit.point);
                    _projectileGameObject.transform.position = projectileHit.point;
                }
                
                //ADDS IMPULSE WHEN A NEW HOOK IS CREATED
                _rigidbody2D.AddForce(_rigidbody2D.velocity * hookImpulse, ForceMode2D.Impulse);
            }
            else if (_rayCastJointPoints.Count > 0)
            {
                //ADDS HOOK JOINS WHEN THE ROPE COLLIDES WITH A WALL
                var nextHit = Physics2D.Linecast(shooterGuide.transform.position,
                        _projectileGameObject.transform.position,wallLayer);
              if (!ReferenceEquals(nextHit.collider,null))
              {
                  var distance = Vector2.Distance(nextHit.transform.position,
                      _rayCastJointPoints.last());
                  if (distance > jointHitLimiar)
                  {
                      if (_rayCastJointPoints.FindAll(vector => vector == nextHit.point).Count == 0)
                      {
                          _rayCastJointPoints.Add(nextHit.point);
                          _projectileGameObject.transform.position = nextHit.point;
                      }
                  }
              }

              //REMOVE HOOK JOINTS
              if (_rayCastJointPoints.Count >= 2)
              {
                  if (_distanceJoint2D.distance.round(1) == 0)
                  {
                      _projectileGameObject.transform.position = _rayCastJointPoints.getAtEnd(2);
                      _rayCastJointPoints.RemoveAt(_rayCastJointPoints.lastIndex());
                  }
                  else
                  {
                      var hitPrevious = Physics2D.Linecast(shooterGuide.transform.position,
                          _rayCastJointPoints.getAtEnd(2),wallLayer);

                      if (ReferenceEquals(hitPrevious.collider, null))
                      {
                          _projectileGameObject.transform.position = _rayCastJointPoints.getAtEnd(2);
                          _rayCastJointPoints.RemoveAt(_rayCastJointPoints.lastIndex());
                      }
                      else
                      {
                          var distance = Vector2.Distance(hitPrevious.point,
                              _rayCastJointPoints.getAtEnd(
                                  2));

                          if (distance.round(1) == 0)
                          {
                              _projectileGameObject.transform.position = _rayCastJointPoints.getAtEnd(2);
                              _rayCastJointPoints.RemoveAt(_rayCastJointPoints.lastIndex());
                          }
                      }
                  }
              }
            }

            _distanceJoint2D.enabled = true;
            _distanceJoint2D.connectedBody = _projectileRigidbody2D;
            _distanceJoint2D.anchor = Vector2.zero;
        }
    }

    //NORMALIZE THE SHOOT DIRECTION (ALWAYS SHOOTS UP)
    private Vector2 NormalizeVelocityDirection(Vector2 normalizedVelocity)
    {
        const float normalizedMinimumUpDirection = 1f;
        const float normalizedMinimumSideDirection = 0.5f;
        var temp = normalizedVelocity;

        if (temp.y < normalizedMinimumUpDirection)
        {
            temp.y = normalizedMinimumUpDirection;

            if (temp.x > 0)
            {
                temp.x = normalizedMinimumSideDirection;
            }
            else if(temp.x < 0)
            {
                temp.x = -normalizedMinimumSideDirection;
            }
        }
        
        return temp;
    }
    
    public Projectile GetProjectile()
    {
        return _projectileScript;
    }

}
