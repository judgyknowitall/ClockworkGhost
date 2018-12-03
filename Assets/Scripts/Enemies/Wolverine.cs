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
        Vector2 ouput;

        var hitPlayer = Physics2D.Raycast(transform.position, tmpDir, Mathf.Infinity, ~(1 << 8));
        if (hitPlayer.collider != null){
            Debug.DrawRay(transform.position, tmpDir, Color.green);
            if (hitPlayer.collider.gameObject.tag == "Player") return tmpDir;
        }

        var maybeHit = Physics2D.Raycast(transform.position, tmpDir, lookDistance, 1 << 8);
        if (maybeHit.collider == null) {
            //Debug.DrawLine(transform.position, transform.position + tmpDir.normalized * 0.1f + tmpDir.normalized * lookDistance, Color.green, 0.5f);
            //Debug.Log(maybeHit.collider);
            ouput = tmpDir;
        }
        else {
            //Debug.DrawLine(transform.position, transform.position + tmpDir.normalized * 0.1f + tmpDir.normalized * lookDistance, Color.red, 0.5f);
            //Debug.Log(maybeHit.collider);
            ouput = Random.insideUnitCircle.normalized;
        }

        if (Vector2.Dot(ouput, Vector2.left) > 0.5f)
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
        }
        else
        {
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
        }

        return ouput;


    }
}
