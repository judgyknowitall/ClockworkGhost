using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolverine : Enemy
{
    [Range(0,10)]
    [SerializeField] private float range;

    [Header("Attack Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator animator;


    [Header("Attack Cooldown Time")]
    [SerializeField] private float attackCooldownLength = 2;
    [SerializeField] private float paceFrequency;

    [SerializeField] private float lookDistance;

    [SerializeField] private uint damage = 15;

    private float attackCooldown = 0;

    protected override void Start()
    {
        base.Start();
        stunned = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (beingKilled) return;
        Attack();
    }

    public void Attack()
    {
        if (beingKilled) { return; }
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
        var tmpDir = player.transform.position - transform.position;

        var maybeHit = Physics2D.Raycast(transform.position + tmpDir, tmpDir, lookDistance);
        if (maybeHit.collider == null) return tmpDir;
        else{
            Debug.Log(maybeHit.collider);
            var tryDirection = Physics2D.Raycast(transform.position, Vector2.up, lookDistance);
            if (tryDirection.collider != null) return Vector2.up;
            tryDirection = Physics2D.Raycast(transform.position, Vector2.down, lookDistance);
            if (tryDirection.collider != null) return Vector2.down;
            tryDirection = Physics2D.Raycast(transform.position, Vector2.left, lookDistance);
            if (tryDirection.collider != null) return Vector2.left;
            tryDirection = Physics2D.Raycast(transform.position, Vector2.right, lookDistance);
            if (tryDirection.collider != null) return Vector2.right;
            else return Vector2.zero;
        }
    }
}
