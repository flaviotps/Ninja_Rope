using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public GameObject shooterGuide;
    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] public DistanceJoint2D distanceJoint2D;
    [SerializeField] public Rigidbody2D rigidbody2D;
    //PROJECTILE
    private GameObject _projectileGameObject;
    private Projectile _projectileScript;
    private Rigidbody2D _projectileRigidbody2D;
    
    private const int Force = 1000;

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
    }

    // Update is called once per frame
    void Update()
    {
        updateLine();
        handleJoint();
    }

    public void FireProjectile()
    {
        _projectileScript.isHooked = false;
        _projectileGameObject.transform.position = transform.position;
        _projectileRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _projectileRigidbody2D.AddForce(_projectileGameObject.transform.up * Force); 
    }

    private void updateLine()
    {
        lineRenderer.SetPosition(0,shooterGuide.transform.position);
        lineRenderer.SetPosition(1,_projectileGameObject.transform.position);
    }


    private void handleJoint()
    {
        if (_projectileScript.isHooked)
        {
            distanceJoint2D.enabled = true;
            distanceJoint2D.connectedBody = _projectileRigidbody2D;
            distanceJoint2D.connectedAnchor = _projectileGameObject.transform.position;
            distanceJoint2D.anchor = Vector2.zero;
        }
        else
        {
            distanceJoint2D.enabled = false;
        }
    }
}
