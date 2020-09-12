using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public Joystick HorizontalJoystick;
    [SerializeField] public Joystick verticalJoystick;
    [SerializeField] public PhysicsMaterial2D hookedBouncyMaterial;
    [SerializeField] public PhysicsMaterial2D airBouncyMaterial;
    [SerializeField] public PhysicsMaterial2D groundedFrictionMaterial;
    
    private Shooter _shooterScript;
    private Movement _movementScript;
    private CircleCollider2D _circleCollider2D;
    private Boolean _isGrounded;
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
        if (HorizontalJoystick.Horizontal > 0f)
        {
            if (!_shooterScript.GetProjectile().isHooked || _shooterScript.GetProjectile().isHooked && _isGrounded)
            {
                _movementScript.MoveRight(HorizontalJoystick.Horizontal);
            } 
            else
            {
                var playerToHookDirection = ((Vector2)_shooterScript.GetProjectile().transform.position - (Vector2) transform.position).normalized;
                var perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                _movementScript.SwingRight(perpendicularDirection);
            }
        }else if (HorizontalJoystick.Horizontal < 0f)
        {
            if (!_shooterScript.GetProjectile().isHooked || _shooterScript.GetProjectile().isHooked && _isGrounded)
            {
                _movementScript.MoveLeft(HorizontalJoystick.Horizontal);
            }
            else
            {
                var playerToHookDirection = ((Vector2)_shooterScript.GetProjectile().transform.position - (Vector2) transform.position).normalized;
                var perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                _movementScript.SwingLeft(perpendicularDirection);
            }
        }

        if (verticalJoystick.Vertical > 0f)
        {
            if (_shooterScript.GetProjectile().isHooked)
            {
                _movementScript.ClimbUp(verticalJoystick.Vertical);
            }
            else
            {
                if (_isGrounded)
                {
                    _movementScript.Jump();
                }
            }
        }else if (verticalJoystick.Vertical < 0f)
        {
            if (_shooterScript.GetProjectile().isHooked)
            {
                _movementScript.ClimbDown(verticalJoystick.Vertical);
            }
        }
    }

    void HandleMaterialChange()
    {
        if (_shooterScript.GetProjectile().isHooked)
        {
            _circleCollider2D.sharedMaterial = hookedBouncyMaterial;
        }
        else if(_isGrounded)
        {
            _circleCollider2D.sharedMaterial = groundedFrictionMaterial;
        }
        else
        {
            _circleCollider2D.sharedMaterial = airBouncyMaterial;
        }
    }
}
