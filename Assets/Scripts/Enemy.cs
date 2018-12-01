using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
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

    private Vector2 DecideMovementDirection();
    #endregion
}
