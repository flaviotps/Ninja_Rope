using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rigidbody2D;
    [SerializeField] public DistanceJoint2D distanceJoint2D;
    [SerializeField] public int moveSpeed = 10;
    [SerializeField] public int climbSpeed = 100;
    [SerializeField] public int impulseForce = 1000;
    [SerializeField] public int jumpForce = 1000;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void swingRight()
    {
        rigidbody2D.AddForce(transform.right * (impulseForce * Time.deltaTime), ForceMode2D.Impulse); 
    }
    
    public void swingLeft()
    {
        rigidbody2D.AddForce(transform.right * (-impulseForce * Time.deltaTime), ForceMode2D.Impulse); 
    }
    
    public void moveRight()
    {
        transform.position += new Vector3(1*moveSpeed * Time.deltaTime,0,0);
    }
    
    public void moveLeft()
    {
        transform.position += new Vector3(-1*moveSpeed * Time.deltaTime,0,0);
    }
    
    public void climbUp()
    {
        distanceJoint2D.distance -= climbSpeed * Time.deltaTime;
        if (distanceJoint2D.distance < 0)
        {
            distanceJoint2D.distance = 0;
        }
    }
    
    public void climbDown()
    {
        distanceJoint2D.distance += climbSpeed * Time.deltaTime;
    }
    
    public void jump()
    {
        rigidbody2D.AddForce(transform.up * (jumpForce * Time.deltaTime), ForceMode2D.Impulse); 
    }
}
