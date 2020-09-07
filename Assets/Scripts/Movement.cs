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
    [SerializeField] public float maximumTilt = 35;
    
    private DistanceJoint2D _distanceJoint2D;
    private Rigidbody2D _rigidbody2D;

    private RaycastHit2D _groundRaycastHit2D;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
  
    }

    public void SwingRight(Vector2 perpendicularDirection)
    {
        var force = perpendicularDirection * swingForce;
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }
    
    public void SwingLeft(Vector2 perpendicularDirection)
    {
            var force = perpendicularDirection * swingForce;
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }
    
    public void MoveRight()
    {
        MoveToDirection(1);
    }
    
    public void MoveLeft()
    {
        MoveToDirection(-1);
    }
    
    public void ClimbUp()
    {
        _distanceJoint2D.distance -= climbSpeed;
    }
    
    public void ClimbDown()
    {
        _distanceJoint2D.distance += climbSpeed;
    }
    
    public void Jump()
    {
        _rigidbody2D.AddForce(transform.up * (jumpForce), ForceMode2D.Impulse); 
    }


    private void MoveToDirection(float directionX)
    {
        Vector2 forceDirection = new Vector2(0,0);
        // Verifique se há chao abaixo.
        RaycastHit2D hitDirection = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance, groundLayer);
        //Desenhando hit
        Debug.DrawRay(transform.position, Vector2.down * rayCastDistance, Color.yellow);
        // Se houver chão abaixo.
        if (hitDirection.collider != null)
        {
            // Desenhando um raio com a diração co chao detectado (Normals)
            Debug.DrawRay(transform.position, hitDirection.normal, Color.white);
           
            // Se isso não apontar para cima, estamos em uma inclinação
            // e deve ajustar nossa forceDirection ...

            // Obtenha um vetor perpendicular ao normal.
            // Isso apontará para cima ao longo da inclinação.
            if (directionX >= 0) // Right
                forceDirection = new Vector2(hitDirection.normal.y, -hitDirection.normal.x);
            else // Left
                forceDirection = new Vector2(-hitDirection.normal.y, hitDirection.normal.x);
        }
        float angle = 0;
       
        // Criando um raio para direção que detecta o angulo do obstáculo a  frente
        RaycastHit2D hitAngle = Physics2D.Raycast(transform.position, forceDirection, 0.5f, groundLayer);
        Debug.DrawRay(transform.position, hitAngle.normal, Color.blue);
        //Pegando angulo do chao
        angle = Vector2.Angle(hitAngle.normal, Vector2.up);
        //Verifica Angulo
        if (angle < maximumTilt)
        {
             // Criando um raio na direção à seguir 
            Debug.DrawRay(transform.position, forceDirection * Mathf.Abs(directionX), Color.green);
            //Move o player
            transform.Translate(forceDirection * (Mathf.Abs(directionX) * moveSpeed));
        }else{
             // Criando um raio na direção à seguir 
            Debug.DrawRay(transform.position, forceDirection * Mathf.Abs(directionX), Color.red);
        }
    }
}
