using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Shooter shooterScript;
    [SerializeField] public Movement movementScript;
    [SerializeField] public BoxCollider2D boxCollider2D;
    private Boolean _isGrounded;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics2D.IsTouchingLayers(boxCollider2D, LayerMask.GetMask("Wall"));
        HandleInput();
    }
    
    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            movementScript.climbUp();
        }
        
        if (Input.GetKey(KeyCode.DownArrow) )
        {
            movementScript.climbDown();
        }
        
        if (Input.GetKey(KeyCode.RightArrow) )
        {
            if (_isGrounded)
            {
                movementScript.moveRight();
            }
            else
            {
                movementScript.swingRight();
            }
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) )
        {
            if (_isGrounded)
            {
                movementScript.moveLeft();
            }
            else
            {
                movementScript.swingLeft();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            shooterScript.FireProjectile();
        }
        
        
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            if (_isGrounded)
            {
                movementScript.jump();
            }
        }
    }
}
