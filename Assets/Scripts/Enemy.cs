using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    private Mover mover;
    private int health;

	void Start ()
    {
        mover = GetComponent<Mover>();	
	}
	
	void FixedUpdate()
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

    public abstract void DoDamage(uint strength);

    #endregion
}
