using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolverine : Enemy
{
    [Range(0,10)]
    [SerializeField] private float range;

    [Header("Attack Sound")]
    public AudioSource audioSource;
    [SerializeField] private Animator animator;
    

    [Header("Attack Cooldown Time")]
    [SerializeField] private float attackCooldownLength = 2;
    [SerializeField] private float paceFrequency;
    [SerializeField] private HitBoxDetector leftAttackCollider;
    [SerializeField] private HitBoxDetector rightAttackCollider;

    [SerializeField] private float lookDistance;

    [SerializeField] private uint damage = 15;
    public LevelManager.Node startRoom;

    /* Stack<Goal> plan = new Stack<Goal>();

    Goal currentGoal;
    GoalData brain;*/

    private float attackCooldown = 0;

    private Vector2 back = Vector2.right;

    private void Awake() {
        
    }

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>();
        stunned = false;
    }

    /*void NextGoal(){
        foreach (var dep in plan){
            //print(dep);
        }
        var top = plan.Peek();
        bool allGood = false;

        while (!allGood){
            allGood = true;
            foreach (var dep in top.dependencies){
                //print(dep + " " + dep.isAchieved);
                if (dep.isAchieved) continue;
                //print(dep);
                plan.Push(dep);
                allGood = false;
            }
            top = plan.Peek();
        }

        plan.Pop();
        currentGoal = plan.Peek();
        //print(currentGoal);
    }*/

    void Update(){
        /*if (startRoom != null && brain == null){
            brain = new GoalData{
                self = transform,
                player = this.player,
                levelManager = FindObjectOfType<LevelManager>(),
                currentRoom = startRoom
            };
            brain.exploredRooms.Add(brain.currentRoom);
            plan.Push(new KillPlayer(brain));
            foreach (var dep in plan){
                //print(dep);
            }
            NextGoal();
            ready = true;
        }*/
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
        if ( animator.GetBool("LeftAttack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("AttackLeft"))
            animator.SetBool("LeftAttack", false);
        if (animator.GetBool("RightAttack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("AttackRight"))
            animator.SetBool("RightAttack", false);
        Attack();
    }

    public void Attack()
    {
        if (beingKilled) { return; }
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            if (leftAttackCollider.playerInMe)
                Swipe(true);
            else if (rightAttackCollider.playerInMe)
                Swipe(false);
        }
    }

    private void Swipe(bool left)
    {
        player.DoDamage(damage);
        if (left)
            animator.SetBool("LeftAttack", true);
        else
            animator.SetBool("RightAttack", true);

        attackCooldown = attackCooldownLength;
    }

    public override void DoDamage(float strength)
    {
        health -= strength;
        if (health < 0) Destroy(this.gameObject);
    }

    public bool CanAttack()
    {
        var playerPos = (Vector2)player.transform.position;
        var tolerance = Vector2.Dot(playerPos.normalized, back);

        return tolerance < 0.5f || stunned;
    }

    protected override Vector2 DecideMovementDirection()
    {
        if (stunned)
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            animator.SetBool("Stun", true);
            return Vector2.zero;
        } 
        
        /*if (!currentGoal.isAchieved && !brain.newDependenciesExist){
            //print("Doing " + currentGoal + " because " + brain.newDependenciesExist);
            return currentGoal.Do();
        }else{
            NextGoal();
            brain.newDependenciesExist = false;
            return Vector2.zero;
        }*/

        var tmpDir = player.transform.position - transform.position;
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
            ouput = Vector2.zero;
        }

        float angle = FindDegree(ouput.x, ouput.y);

        Debug.DrawRay(transform.position, ouput.normalized*2, Color.red, .75f, false);

        if (angle >= 90 && angle <= 180)
        {
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        }
        else
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
        }

        return ouput;
    }

    public static float FindDegree(float x, float y)
    {
        float value = (float)((Mathf.Atan2(x, y) / System.Math.PI) * 180f);
        if (value < 0) value += 360f;

        return value;
    }
}
