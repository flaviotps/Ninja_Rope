using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rigidbody2D;
    [SerializeField] public DistanceJoint2D distanceJoint2D;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float moveSpeed = 0.5f;
    [SerializeField] public int climbSpeed = 100;
    [SerializeField] public int impulseForce = 100;
    [SerializeField] public int jumpForce = 100;
    [SerializeField] public int maxVelocity = 20;
    
    private float _rayCastDistance = 0.5f;
    private Vector2 _direction = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitDirection = Physics2D.Raycast(transform.position, Vector2.down, _rayCastDistance, groundLayer);

        if (hitDirection.collider != null)
        {
            Debug.DrawRay(transform.position, hitDirection.normal, Color.white);
            Debug.DrawRay(transform.position, Vector2.down * _rayCastDistance, Color.yellow);
            _direction = hitDirection.normal;
        }

        if (rigidbody2D.velocity.magnitude > maxVelocity)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxVelocity;
        }

    }

    public void SwingRight(Projectile projectile)
    {
        rigidbody2D.AddForce(transform.right * (impulseForce * Time.deltaTime), ForceMode2D.Impulse); 
    }
    
    public void SwingLeft(Projectile projectile)
    {
        rigidbody2D.AddForce(transform.right * (-impulseForce * Time.deltaTime), ForceMode2D.Impulse); 
    }
    
    public void MoveRight()
    {
        var forceDirection = new Vector2(_direction.y, -_direction.x);
        transform.Translate(forceDirection  * moveSpeed);
    }
    
    public void MoveLeft()
    {
        var forceDirection = new Vector2(-_direction.y, _direction.x);
        transform.Translate(forceDirection * moveSpeed);
    }
    
    public void ClimbUp()
    {
        distanceJoint2D.distance -= climbSpeed * Time.deltaTime;
        if (distanceJoint2D.distance < 0)
        {
            distanceJoint2D.distance = 0;
        }
    }
    
    public void ClimbDown()
    {
        distanceJoint2D.distance += climbSpeed * Time.deltaTime;
    }
    
    public void Jump()
    {
        rigidbody2D.AddForce(transform.up * (jumpForce * Time.deltaTime), ForceMode2D.Impulse); 
    }
}
