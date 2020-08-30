using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Shooter shooterScript;
    [SerializeField] public Movement movementScript;
    private Boolean _isSwinging = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            if (_isSwinging)
            {
                movementScript.swingRight();
            }
            else
            { 
                movementScript.moveRight();
            }
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) )
        {
            if (_isSwinging)
            {
                movementScript.swingLeft();
            }
            else
            {
                movementScript.moveLeft();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            shooterScript.FireProjectile();
        }
    }

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("OnCollisionEnter2D" + other.gameObject.layer);
        if (other.collider.IsTouchingLayers(LayerMask.GetMask("Wall")))
        {
            _isSwinging = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("OnCollisionExit2D" + other.gameObject.layer);
        if (other.collider.IsTouchingLayers(LayerMask.GetMask("Wall")))
        {
            _isSwinging = true;
        }
    }*/
}
