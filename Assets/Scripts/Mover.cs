using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    public float speed = 1;

    private new Rigidbody2D rigidbody;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int displayOrderOffset = 0;

    void Start ()
    {
        rigidbody = GetComponent<Rigidbody2D>();	
	}
	
	void FixedUpdate()
    {
        spriteRenderer.sortingOrder = (Mathf.RoundToInt(transform.position.y * 100f) * -1)+displayOrderOffset;
    }

    public void Move(Vector2 direction)
    {
        rigidbody.velocity = direction.normalized * speed;
    }
}
