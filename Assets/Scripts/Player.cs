using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Player : MonoBehaviour
{
    private Mover mover;
    private uint ether;

    void Start ()
    {
        mover = GetComponent<Mover>();
	}

    void Update ()
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

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movementDirection += Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movementDirection += Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movementDirection += Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
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
}
