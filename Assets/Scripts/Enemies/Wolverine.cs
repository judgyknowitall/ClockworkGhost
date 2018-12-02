using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolverine : Enemy
{
    [Range(0,10)]
    [SerializeField] private float range;

    [Header("Attack Sound")]
    [SerializeField] private AudioSource audioSource;

    [Header("Attack Cooldown Time")]
    [SerializeField] private float attackCooldownLength = 2;

    [SerializeField] private uint damage = 15;

    private float attackCooldown = 0;

    protected override void Start()
    {
        base.Start();
        stunned = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Attack();
    }

    public void Attack()
    {
        attackCooldown -= Time.deltaTime;
        if (Vector2.Distance(transform.position, player.transform.position) <= range && attackCooldown <= 0)
        {
            player.DoDamage(damage);
            if (audioSource != null)
                audioSource.Play();
            attackCooldown = attackCooldownLength;
            Debug.Log("WOOF!");
        }
    }

    public override void DoDamage(uint strength)
    {
        health -= strength;
        if (health < 0) Destroy(this.gameObject);
    }

    protected override Vector2 DecideMovementDirection()
    {
        return Vector2.zero;
    }
}
