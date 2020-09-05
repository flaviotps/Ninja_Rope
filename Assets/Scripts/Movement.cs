using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   
  
  
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float moveSpeed = 0.4f;
    [SerializeField] public float rayCastDistance = 2;
    [SerializeField] public int climbSpeed = 100;
    [SerializeField] public int swingForce = 50;
    [SerializeField] public int jumpForce = 250;
    
    private DistanceJoint2D _distanceJoint2D;
    private Rigidbody2D _rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SwingRight(Vector2 perpendicularDirection)
    {
        var force = perpendicularDirection * swingForce;
        _rigidbody2D.AddForce(force, ForceMode2D.Force);
    }
    
    public void SwingLeft(Vector2 perpendicularDirection)
    {
            var force = perpendicularDirection * swingForce;
            _rigidbody2D.AddForce(force, ForceMode2D.Force);
    }
    
    public void MoveRight()
    {
        var hitDirection = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, groundLayer);

        
        if (!ReferenceEquals(hitDirection.collider, null))
        {
            var forceDirection = new Vector2(hitDirection.normal.y, -hitDirection.normal.x);
            transform.Translate(forceDirection  * moveSpeed);
        }
    }
    
    public void MoveLeft()
    {
        var hitDirection = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, groundLayer);

        if (!ReferenceEquals(hitDirection.collider, null))
        {
            var forceDirection = new Vector2(-hitDirection.normal.y, hitDirection.normal.x);
            transform.Translate(forceDirection * moveSpeed);
        }
    }
    
    public void ClimbUp()
    {
        _distanceJoint2D.distance -= climbSpeed * Time.deltaTime;
    }
    
    public void ClimbDown()
    {
        _distanceJoint2D.distance += climbSpeed * Time.deltaTime;
    }
    
    public void Jump()
    {
        _rigidbody2D.AddForce(transform.up * (jumpForce * Time.deltaTime), ForceMode2D.Impulse); 
    }
}
