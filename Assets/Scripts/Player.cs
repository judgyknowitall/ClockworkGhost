using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Mover mover;
    private uint ether;

    void Start ()
    {
		
	}

    void Update ()
    {
        Move();
	}

    #region Movement

    private void Move()
    {
        Vector2 movementDirection = DecideMovementDirection();
        mover.Move(movementDirection);
    }

    private Vector2 DecideMovementDirection()
    {
        Vector2 movementDirection = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Bite();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Up))
        {
            movementDirection += Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.Right))
        {
            movementDirection += Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.Down))
        {
            movementDirection += Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.Left))
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
