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
    [SerializeField] public float jointHitLimiar = 0.1f;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public LayerMask projectileLayer;

    //PLAYER
    private DistanceJoint2D _distanceJoint2D;
    private LineRenderer _lineRenderer;
    private BoxCollider2D _boxCollider2D;
    private Rigidbody2D _rigidbody2D;

    //PROJECTILE
    private GameObject _projectileGameObject;
    private Projectile _projectileScript;
    private Rigidbody2D _projectileRigidbody2D;
    private SpriteShapeRenderer _spriteShapeRenderer;
    
    private readonly List<Vector2> _rayCastJointPoints = new List<Vector2>();

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _projectileGameObject = Instantiate(projectilePrefab, shooterGuide.transform.position, shooterGuide.transform.rotation);
        _projectileRigidbody2D = _projectileGameObject.GetComponent<Rigidbody2D>();
        _projectileScript = _projectileGameObject.GetComponent<Projectile>();
        _spriteShapeRenderer = _projectileGameObject.GetComponent<SpriteShapeRenderer>();
        Physics2D.IgnoreCollision(_boxCollider2D, _projectileGameObject.GetComponent<CircleCollider2D>());
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
            _rayCastJointPoints.Clear();
            _projectileScript.isHooked = false;
            _spriteShapeRenderer.enabled = false;
            _rigidbody2D.velocity *= new Vector2(1, 0);
        }
        else
        {
            _spriteShapeRenderer.enabled = true;
            _projectileGameObject.transform.position = shooterGuide.transform.position;
            _projectileRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _projectileRigidbody2D.AddForce(_projectileGameObject.transform.up * projectileForce);
        }
    }

    private void UpdateLine()
    {
        if (_projectileScript.isHooked)
        {
            //ALOCATE ENOUGH POSITIONS TO ALL SEGMENTS + LAST SEGMENT
            _lineRenderer.positionCount = _rayCastJointPoints.Count + 1;
            //DRAW ALL LINES SEGMENTS
            for (var index = 0; index < _rayCastJointPoints.Count; index++)
            {
                _lineRenderer.SetPosition(index, _rayCastJointPoints[index]);
            }
            //DRAW THE LAST LINE SEGMENT (CLOSEST HOOK JOINT TO PLAYER)
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, shooterGuide.transform.position.round2d());
        }
        else
        {
            _lineRenderer.positionCount = 0;
            _rayCastJointPoints.Clear();
        }

        if (_rayCastJointPoints.Count > 2)
        {
            Debug.DrawLine(shooterGuide.transform.position.round2d(), _rayCastJointPoints.getAtEnd( 2).round2d());
        }
    }


    private void HandleJoint()
    {
        if (_projectileScript.isHooked)
        {
            //ADDS THE INITIAL HOOK POSITION TO THE LIST
            if (_rayCastJointPoints.Count == 0)
            {
                var projectileHit = Physics2D.Linecast(shooterGuide.transform.position.round2d(),
                    _projectileGameObject.transform.position.round2d(),projectileLayer);

                if (!ReferenceEquals(projectileHit.collider, null))
                {
                    _rayCastJointPoints.Add(projectileHit.point.round2d());
                    _projectileGameObject.transform.position = projectileHit.point.round2d();
                }
            }
            else if (_rayCastJointPoints.Count > 0)
            {
                //ADDS HOOK JOINS WHEN THE ROPE COLLIDES WITH A WALL
                var nextHit = Physics2D.Linecast(shooterGuide.transform.position.round2d(),
                        _projectileGameObject.transform.position.round2d(),wallLayer);
              if (!ReferenceEquals(nextHit.collider,null))
              {
                  var distance = Vector2.Distance(nextHit.transform.position.round2d(),
                      _rayCastJointPoints.last().round2d());
                  if (distance > jointHitLimiar)
                  {
                      if (_rayCastJointPoints.FindAll(vector => vector == nextHit.point.round2d()).Count == 0)
                      {
                          _rayCastJointPoints.Add(nextHit.point.round2d());
                          _projectileGameObject.transform.position = nextHit.point.round2d();
                      }
                  }
              }

              //REMOVE HOOK JOINTS
              if (_rayCastJointPoints.Count >= 2)
              {
                  if (_distanceJoint2D.distance.round(0) == 0)
                  {
                      _projectileGameObject.transform.position = _rayCastJointPoints.getAtEnd(2).round2d();
                      _rayCastJointPoints.RemoveAt(_rayCastJointPoints.lastIndex());
                  }
                  else
                  {
                      var hitPrevious = Physics2D.Linecast(shooterGuide.transform.position.round2d(),
                          _rayCastJointPoints.getAtEnd(2).round2d(),wallLayer);

                      if (ReferenceEquals(hitPrevious.collider, null))
                      {
                          _projectileGameObject.transform.position = _rayCastJointPoints.getAtEnd(2).round2d();
                          _rayCastJointPoints.RemoveAt(_rayCastJointPoints.lastIndex());
                      }
                      else
                      {
                          var distance = Vector2.Distance(hitPrevious.point,
                              _rayCastJointPoints.getAtEnd(
                                  2).round2d());

                          if (distance.round(0) == 0)
                          {
                              _projectileGameObject.transform.position = _rayCastJointPoints.getAtEnd(2).round2d();
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
        else if(!_projectileScript.isHooked)
        {
            _distanceJoint2D.enabled = false;
            _distanceJoint2D.connectedBody = null;
            _distanceJoint2D.anchor = Vector2.zero;
        }
    }

    public Projectile GetProjectile()
    {
        return _projectileScript;
    }
}
