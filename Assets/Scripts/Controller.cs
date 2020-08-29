using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Rigidbody2D rigidbody2D;
    [SerializeField] public DistanceJoint2D distanceJoint2D;
    [SerializeField] public int moveSpeed = 10;
    [SerializeField] public int climbSpeed = 100;
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
        if (Input.GetKey(KeyCode.RightArrow) )
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            distanceJoint2D.distance -= climbSpeed * Time.deltaTime;
            if (distanceJoint2D.distance < 0)
            {
                distanceJoint2D.distance = 0;
            }
        }
        
        if (Input.GetKey(KeyCode.DownArrow) )
        {
            distanceJoint2D.distance += climbSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) )
        {
            transform.position += Vector3.right * -moveSpeed * Time.deltaTime;
        }
    }
}
