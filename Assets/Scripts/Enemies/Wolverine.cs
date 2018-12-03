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

    Queue<Goal> plan;

    private float attackCooldown = 0;

    private Vector2 back = Vector2.right;

    protected override void Start()
    {
        base.Start();
        stunned = false;
    }

    void Update(){
        if (stunned) print ("I am so stunned right now");
    }

    protected override void FixedUpdate()
    {        
        if (animator.GetBool("Left")){
            back = Vector2.right;
        }else{
            back = Vector2.left;
        }
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
        }
    }  

    public override void DoDamage(float strength)
    {
        health -= strength;
        if (health < 0) Destroy(this.gameObject);
    }

    public bool CanAttack(){
        var playerPos = (Vector2)player.transform.position;
        var tolerance = Vector2.Dot(playerPos.normalized, back);

        return tolerance < 0.5f || stunned;
    }

    protected override Vector2 DecideMovementDirection()
    {
        if (stunned) return Vector2.zero;
        return Vector2.zero;
        /*var tmpDir = player.transform.position - transform.position;
        Vector2 ouput;

        var hitPlayer = Physics2D.Raycast(transform.position, tmpDir, Mathf.Infinity, ~(1 << 8));
        if (hitPlayer.collider != null){
            //Debug.DrawRay(transform.position, tmpDir, Color.green);
            //Debug.Log(hitPlayer.collider.gameObject.tag);
            if (hitPlayer.collider.gameObject.tag == "Player") return tmpDir;
        }

        var maybeHit = Physics2D.Raycast(transform.position, tmpDir, lookDistance, 1 << 8);
        if (maybeHit.collider == null) {
            //Debug.DrawLine(transform.position, transform.position + tmpDir.normalized * 0.1f + tmpDir.normalized * lookDistance, Color.green, 0.5f);
            //Debug.Log(maybeHit.collider);
            ouput = tmpDir;
        }
        else {
            //Debug.Drprivate Vector2 backSide = Vector2.left;awLine(transform.position, transform.position + tmpDir.normalized * 0.1f + tmpDir.normalized * lookDistance, Color.red, 0.5f);
            //Debug.Log(maybeHit.collider);
            ouput = Random.insideUnitCircle.normalized;
        }

        if (Vector2.Dot(ouput, Vector2.left) > 0.5f)
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
            back = Vector2.left;

        }
        else
        {
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
            back = Vector2.right;
        }

        return ouput;*/
    }
}
