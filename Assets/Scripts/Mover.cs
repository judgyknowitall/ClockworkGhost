using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    public float speed = 1;

    private new Rigidbody2D rigidbody;
    //private 

    void Start ()
    {
        rigidbody = GetComponent<Rigidbody2D>();	
	}
	
	void FixedUpdate()
    {
        sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    public void Move(Vector2 direction)
    {
        rigidbody.velocity = direction.normalized * speed;
    }
}
