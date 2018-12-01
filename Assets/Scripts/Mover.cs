using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    public float speed = 1;

    private new Rigidbody2D rigidbody;

    void Start ()
    {
        rigidbody = GetComponent<Rigidbody2D>();	
	}
	
	void FixedUpdate()
    {
		
	}

    public void Move(Vector2 direction)
    {
        rigidbody.velocity = direction.normalized * speed;
    }
}
