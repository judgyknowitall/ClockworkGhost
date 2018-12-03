using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(Mover))]
public class Player : MonoBehaviour, IDamageable
{
    private Mover mover;
    [Header("Componenets and Objects!")]
    [SerializeField] private Animator[] animators;
    [SerializeField] private GameObject shadow;
    [SerializeField] private Head head;

    [Header("Attributes")]
    public float ether;

    private bool biting = false;
    [Header("Bite")]
    [SerializeField] private float biteCircleWidth = 0.1f;
    [SerializeField] private float jumpPauseTime;
    [SerializeField] private int jumpTimeSteps;
    [SerializeField] private float biteCircleMaxDistance;
    [SerializeField] private float etherFromBite;


    private bool bats = false;
    [Header("Bats")]
    [SerializeField] private float batsTime;
    [SerializeField] private float speedMultiplier = 5;
    private float batsEndTime;
    [SerializeField] private float batsEtherCost = 50;
    [SerializeField] private LayerMask yourLayer;
    [SerializeField] private LayerMask batIgnoreLayer;

    [Header("Stunning")]
    [SerializeField] private float stunEtherCost = 60;
    [SerializeField] private float stunTime = 10;
    [SerializeField] private float stunRadius = 10;

    public event System.Action OnDeath;

    void Start ()
    {
        mover = GetComponent<Mover>();
    }

    void FixedUpdate ()
    {
        Move();
        UseAbilities();
        if (bats) Bats();
        Death();
    }

    private void Death()
    {
        if (ether <= 0)
        {
            if (OnDeath != null)
                OnDeath();
        }
    }

    #region Abilities
    void UseAbilities()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !biting)
        {
            Bats();
            ConsumeEther(batsEtherCost);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !biting)
        {
            Debug.Log("Wow! Stunning!");
            Stun();
            ConsumeEther(stunEtherCost);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Ability 3!");
            ConsumeEther(100);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Ability 4!");
            ConsumeEther(200);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Ability 5!");
            ConsumeEther(400);
        }
    }

    public void Bats()
    {
        if (!bats)
        {
            Physics2D.IgnoreLayerCollision(yourLayer.layermask_to_layer(), batIgnoreLayer.layermask_to_layer(), true);
            bats = true;
            mover.speed = mover.speed * speedMultiplier;
            foreach (Animator animator in animators)
            {
                animator.SetBool("Bat", true);
            }
            shadow.SetActive(false);
            batsEndTime = Time.time + batsTime;
        }

        if (batsEndTime < Time.time)
        {
            Physics2D.IgnoreLayerCollision(yourLayer.layermask_to_layer(), batIgnoreLayer.layermask_to_layer(), false);
            shadow.SetActive(true);
            bats = false;
            mover.speed = mover.speed / speedMultiplier;
            foreach (Animator animator in animators)
            {
                animator.SetBool("Bat", false);
            }
        }
    }
    
    public void Stun()
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("StunFlash", true);
        }
        var enemyHits = Physics2D.OverlapCircleAll((Vector2)transform.position, stunRadius);
        var enemies = 
            (from e in enemyHits
            select e.GetComponent<Wolverine>()).ToArray();
        foreach (var hit in enemies){
            if (hit == null) continue;
        }
        foreach (var e in enemies){
            if (e == null) continue;
            e.stunned = true;
            StartCoroutine(EndStun(e, stunTime));
        }
    }

    IEnumerator EndStun(Wolverine w, float waitForSeconds){
        yield return new WaitForSeconds(waitForSeconds);
        w.stunned = false;
    }
    #endregion

    #region Movement

    private void Move()
    {
        if (biting) return;

        if (Input.GetKeyDown(KeyCode.Space) && !bats)
        {
            BiteAttempt();
            return;
        }

        Vector2 movementDirection = DecideMovementDirection();
        mover.Move(movementDirection);
    }

    private Vector2 DecideMovementDirection()
    {
        Vector2 movementDirection = Vector2.zero;

        foreach (Animator animator in animators)
        {
            animator.SetBool("Left", Input.GetKey(KeyCode.A));
            animator.SetBool("Right", Input.GetKey(KeyCode.D));
            animator.SetBool("Up", Input.GetKey(KeyCode.W));
            animator.SetBool("Down", Input.GetKey(KeyCode.S));

            if (Input.GetKey(KeyCode.W))
            {
                movementDirection += Vector2.up;
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
            }
            if (Input.GetKey(KeyCode.D))
            {
                movementDirection += Vector2.right;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movementDirection += Vector2.down;
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
            }
            if (Input.GetKey(KeyCode.A))
            {
                movementDirection += Vector2.left;
            }

            if ((Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)))
            {
                animator.SetBool("Left", false);
                animator.SetBool("Right", false);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
            }
        }

        return movementDirection.normalized;
    }

    #region Bite

    private void BiteAttempt()
    {
        RaycastHit2D[] spherecastResults = Physics2D.CircleCastAll(transform.position, biteCircleWidth, head.direction, biteCircleMaxDistance);
        RaycastHit2D enemyToHit = new RaycastHit2D();
        bool enemyFound = false;
        foreach (RaycastHit2D hit in spherecastResults)
        {
            if (hit.collider == null || hit.collider.gameObject.GetComponent<Wolverine>() == null || !hit.collider.gameObject.GetComponent<Wolverine>().CanAttack())
            {
                continue;
            }
            enemyFound = true;
            enemyToHit = hit;
            break;
        }
        if (enemyFound)
            Bite(enemyToHit);
        else
        {
            print("No Bite Target Found");
            return;
        }
    }

    private void Bite(RaycastHit2D spherecastResult)
    {
        biting = true;

        spherecastResult.collider.isTrigger = true;
        mover.displayOrderOffset += 1;
        spherecastResult.collider.gameObject.GetComponent<Enemy>().beingKilled = true;

        Vector2 enemyPosition = spherecastResult.transform.position;
        Vector2 playerPosition = transform.position;

        IEnumerator lungeAnimation = Jump(enemyPosition, playerPosition, spherecastResult);
        StartCoroutine(lungeAnimation);
    }

    private IEnumerator Jump(Vector2 enemyPosition, Vector2 playerPosition, RaycastHit2D spherecastResult)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("RedFlash", true);
        }
        for (int i = 0; i < jumpPauseTime; i++)
        {
            yield return new WaitForSeconds(1); 
        }

        float distancePerFrame = Vector2.Distance(enemyPosition, playerPosition) / jumpTimeSteps;
        Vector2 direction = (enemyPosition - playerPosition).normalized;

        float angle = FindDegree(direction.x, direction.y);

        print(angle);

        if (angle >= 315 && angle<= 360 || angle >=0 && angle < 45)
        {
            print("Right!");
            foreach (Animator animator in animators)
            {
                animator.SetBool("RightJump", true);
            }
        }
        else if (angle >= 45 && angle < 135)
        {
            print("UP!");
            foreach (Animator animator in animators)
            {
                animator.SetBool("UpJump", true);
            }
        }
        else if (angle >= 135 && angle < 225)
        {
            print("LEft!");
            foreach (Animator animator in animators)
            {
                animator.SetBool("LeftJump", true);
            }
        }
        else if (angle >= 225 && angle < 315)
        {
            print("Down!");
            foreach (Animator animator in animators)
            {
                animator.SetBool("DownJump", true);
            }
        }

        foreach (Animator animator in animators)
        {
            animator.SetBool("RedFlash", false);
        }

        for (int i = 0; i < jumpTimeSteps; i++)
        {
            transform.position = playerPosition + (direction * (distancePerFrame * i));
            yield return null;
        }

        FinishBite(spherecastResult);
    }

    public static float FindDegree(float x, float y)
    {
        float value = (float)((Mathf.Atan2(x, y) / System.Math.PI) * 180f);
        if (value < 0) value += 360f;

        return value;
    }

    private void FinishBite(RaycastHit2D spherecastResult)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("RightJump", false);
            animator.SetBool("UpJump", false);
            animator.SetBool("LeftJump", false);
            animator.SetBool("DownJump", false);
        }
        spherecastResult.collider.gameObject.GetComponent<Enemy>().stunned=true;
        Destroy(spherecastResult.collider.gameObject);

        //Once dead ??
        ether += etherFromBite;
        biting = false;
        mover.displayOrderOffset -= 1;
    }

    #endregion

    #endregion

    #region Ether
    public void DoDamage(float strength)
    {
        if (bats) return;
        ether = ether - strength;
    }

    public void ConsumeEther(float cost)
    {
        ether = ether - cost;
    }
    #endregion
}
