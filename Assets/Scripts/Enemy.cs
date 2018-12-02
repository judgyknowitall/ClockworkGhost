using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    private Mover mover;
    protected float health;
    protected Player player;

    public bool stunned = false;
    public bool beingKilled = false;

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    protected virtual void Start ()
    {
        mover = GetComponent<Mover>();
	}
	
	protected virtual void FixedUpdate()
    {
        if (stunned) { mover.Move(Vector2.zero); }
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
