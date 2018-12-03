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
    [SerializeField]
    private float paceFrequency;

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
        Vector2 direction = Vector2.left;
        if (Mathf.Sin(paceFrequency * Time.time) > 0)
        {
            direction = direction * -1;
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
        }
        else
        {
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        }

        return direction;
    }
}
