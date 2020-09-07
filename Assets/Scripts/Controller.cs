﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public PhysicsMaterial2D bouncyMaterial;
    [SerializeField] public PhysicsMaterial2D frictionMaterial;
    
    private Shooter _shooterScript;
    private Movement _movementScript;
    private CircleCollider2D _circleCollider2D;
    [SerializeField] public Boolean _isGrounded;
    void Start()
    {
        _shooterScript = GetComponent<Shooter>();
        _movementScript = GetComponent<Movement>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _isGrounded = Physics2D.IsTouchingLayers(_circleCollider2D,groundLayer);
        HandleInput();
        HandleMaterialChange();
    }
    
    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            if (_shooterScript.GetProjectile().isHooked)
            {
                _movementScript.ClimbUp();
            }
        }
        
        if (Input.GetKey(KeyCode.DownArrow) )
        {
            if (_shooterScript.GetProjectile().isHooked)
            {
                _movementScript.ClimbDown();
            }
        }
        
        if (Input.GetKey(KeyCode.RightArrow) )
        {
            if (!_shooterScript.GetProjectile().isHooked)
            {
                _movementScript.MoveRight();
            } 
            else
            {
                var playerToHookDirection = ((Vector2)_shooterScript.GetProjectile().transform.position - (Vector2) transform.position).normalized;
                var perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                _movementScript.SwingRight(perpendicularDirection);
            }

        }
        
        if (Input.GetKey(KeyCode.LeftArrow) )
        {
            if (!_shooterScript.GetProjectile().isHooked)
            {
                _movementScript.MoveLeft();
            }
            else
            {
                var playerToHookDirection = ((Vector2)_shooterScript.GetProjectile().transform.position - (Vector2) transform.position).normalized;
                var perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                _movementScript.SwingLeft(perpendicularDirection);
            }
        }
        
        if (Input.GetKey(KeyCode.Space) )
        {
            _shooterScript.FireProjectile();
        }
        
        
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            if (_isGrounded && !_shooterScript.GetProjectile().isHooked)
            {
                _movementScript.Jump();
            }
        }
    }

    void HandleMaterialChange()
    {
        if (_shooterScript.GetProjectile().isHooked)
        {
            _circleCollider2D.sharedMaterial = bouncyMaterial;
        }
        else
        {
            _circleCollider2D.sharedMaterial = frictionMaterial;
        }
    }
}
