using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour, IDamageable
{
    private Mover mover;
    private Animator animator;
    public float ether;

    void Start ()
    {
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate ()
    {
        Move();
	}

    #region Movement

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Bite();
            return;
        }

        Vector2 movementDirection = DecideMovementDirection();
        mover.Move(movementDirection);
    }

    private Vector2 DecideMovementDirection()
    {
        Vector2 movementDirection = Vector2.zero;

        animator.SetBool("Left", Input.GetKey(KeyCode.LeftArrow));
        animator.SetBool("Right", Input.GetKey(KeyCode.RightArrow));
        animator.SetBool("Up", Input.GetKey(KeyCode.UpArrow));
        animator.SetBool("Down", Input.GetKey(KeyCode.DownArrow));

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementDirection += Vector2.up;
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection += Vector2.right;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementDirection += Vector2.down;
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirection += Vector2.left;
        }

        return movementDirection.normalized;
    }

    private void Bite()
    {
        return;
    }

    #endregion

    public void DoDamage(uint strength)
    {
        ether = ether - strength;
    }

    public void ConsumeEther(uint cost)
    {
        ether = ether - cost;
    }
}
