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
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public PhysicsMaterial2D bouncyMaterial;
    [SerializeField] public PhysicsMaterial2D frictionMaterial;
    private Boolean _isGrounded;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _isGrounded = Physics2D.IsTouchingLayers(boxCollider2D,groundLayer);
        HandleInput();
        HandleMaterialChange();
    }
    
    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            if (shooterScript.GetProjectile().isHooked)
            {
                movementScript.ClimbUp();
            }
        }
        
        if (Input.GetKey(KeyCode.DownArrow) )
        {
            if (shooterScript.GetProjectile().isHooked)
            {
                movementScript.ClimbDown();
            }
        }
        
        if (Input.GetKey(KeyCode.RightArrow) )
        {
            if (!shooterScript.GetProjectile().isHooked)
            {
                movementScript.MoveRight();
            } 
            else
            {
                movementScript.SwingRight(shooterScript.GetProjectile());
            }

        }
        
        if (Input.GetKey(KeyCode.LeftArrow) )
        {
            if (!shooterScript.GetProjectile().isHooked)
            {
                movementScript.MoveLeft();
            }
            else
            {
                movementScript.SwingLeft(shooterScript.GetProjectile());
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            shooterScript.FireProjectile();
        }
        
        
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            if (_isGrounded && !shooterScript.GetProjectile().isHooked)
            {
                movementScript.Jump();
            }
        }
    }

    void HandleMaterialChange()
    {
        if (shooterScript.GetProjectile().isHooked)
        {
            boxCollider2D.sharedMaterial = bouncyMaterial;
        }
        else
        {
            boxCollider2D.sharedMaterial = frictionMaterial;
        }
    }
}
