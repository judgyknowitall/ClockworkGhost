using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public abstract class Enemy : MonoBehaviour
{
    private Mover mover;

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
        Vector2 movementDirection = DecideMovementDirection();
        mover.Move(movementDirection);
    }

    protected abstract Vector2 DecideMovementDirection();
    #endregion
}
