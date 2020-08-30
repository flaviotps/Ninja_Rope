using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D _rigidbody2D;
    private const string HookTag = "hookable";
    public Boolean isHooked ;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals(HookTag))
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            isHooked = true;
        }
     
    }
}
