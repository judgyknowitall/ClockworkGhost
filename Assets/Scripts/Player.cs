using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour, IDamageable
{
    private Mover mover;
    private Animator animator;
    public float ether;

    [SerializeField]
    private Head head;

    private bool biting = false;

    [SerializeField] private float biteCircleWidth = 0.1f;
    [SerializeField] private int jumpPauseTime;
    [SerializeField] private int jumpTimeSteps;
    [SerializeField] private float biteCircleMaxDistance;

    public AnimationCurve jumpCurve = AnimationCurve.Linear(0, 0, 10, 10);

    void Start ()
    {
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate ()
    {
        Move();
        UseAbilities();
	}

    #region Abilities
    void UseAbilities()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Ability 1!");
            ConsumeEther(50);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Ability 2!");
            ConsumeEther(75);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Ability 3!");
            ConsumeEther(100);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Ability 4!");
            ConsumeEther(200);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Ability 5!");
            ConsumeEther(400);
        }
    }
    #endregion

    #region Movement

    private void Move()
    {
        if (biting) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Space");
            BiteAttempt();
            return;
        }

        Vector2 movementDirection = DecideMovementDirection();
        mover.Move(movementDirection);
    }

    private Vector2 DecideMovementDirection()
    {
        Vector2 movementDirection = Vector2.zero;

        animator.SetBool("Left", Input.GetKey(KeyCode.LeftArrow));
        animator.SetBool("Right", Input.GetKey(KeyCode.RightArrow));
        animator.SetBool("Up", Input.GetKey(KeyCode.UpArrow));
        animator.SetBool("Down", Input.GetKey(KeyCode.DownArrow));

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementDirection += Vector2.up;
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection += Vector2.right;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementDirection += Vector2.down;
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirection += Vector2.left;
        }

        return movementDirection.normalized;
    }


    private void BiteAttempt()
    {
        print("Attempting Bite");
        Debug.DrawRay(transform.position, head.direction.normalized * biteCircleMaxDistance, Color.red, 5f);
        RaycastHit2D[] spherecastResults = Physics2D.CircleCastAll(transform.position, biteCircleWidth, head.direction, biteCircleMaxDistance);
        RaycastHit2D enemyToHit = new RaycastHit2D();
        bool enemyFound = false;
        foreach (RaycastHit2D hit in spherecastResults)
        {
            if (hit.collider == null || hit.collider.gameObject.GetComponent<Enemy>() == null || !hit.collider.gameObject.GetComponent<Enemy>().stunned)
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

        print("Begining Bite");

        spherecastResult.collider.isTrigger = true;

        Vector2 enemyPosition = spherecastResult.transform.position;
        Vector2 playerPosition = transform.position;

        IEnumerator lungeAnimation = Jump(enemyPosition, playerPosition);
        print("Begining Jump");
        StartCoroutine(lungeAnimation);
    }

    private IEnumerator Jump(Vector2 enemyPosition, Vector2 playerPosition)
    {
        print("Breif Pause");
        for (int i = 0; i < jumpPauseTime; i++)
        {
            yield return new WaitForSeconds(1); 
        }
        print("Done Pause");

        float distancePerFrame = Vector2.Distance(enemyPosition, playerPosition) / jumpTimeSteps;
        Vector2 direction = (enemyPosition - playerPosition).normalized;

        print("Jumping");
        for (int i = 0; i < jumpTimeSteps; i++)
        {
            print("Jump step" + i);
            transform.position = playerPosition + (direction * (distancePerFrame * i));
            yield return null;
        }

        biting = false;
        print("Jump Complete");
    }

    //private void Bite()
    //{
    //    if (biting) return;

    //    print("Biting attempt");

    //    var hits = Physics2D.CircleCastAll(transform.position, biteRange, Vector2.zero);
    //    RaycastHit2D closest = new RaycastHit2D();
    //    var shortestDistance = Mathf.Infinity;
    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        if (hit.transform.gameObject.GetComponent<Enemy>() == null)
    //            continue;

    //        var hitDistance = Vector2.Distance(hit.transform.position, transform.position);
    //        if (hitDistance < shortestDistance)
    //        {
    //            shortestDistance = hitDistance;
    //            closest = hit;
    //        }
    //    }

    //    if (shortestDistance == Mathf.Infinity)
    //        return;

    //    print("Target Found");

    //    transform.position = closest.transform.position;

    //    ether += 10;

    //    closest.transform.gameObject.GetComponent<Enemy>().DoDamage(999);

    //Vector2 movementDirection = (closest.transform.position - transform.position).normalized;
    //mover.speed = 0;
    //mover.Move(movementDirection);
    //biting = true;

    //IEnumerator coroutine = BiteJump(closest, shortestDistance, movementDirection, transform.position);
    //StartCoroutine(coroutine);
    //}

    //private IEnumerator BiteJump(RaycastHit2D closest, float shortestDistance, Vector2 enemyPrevPos, Vector2 movementDirection)
    //{
    //    print("Begining Jump Coroutine");
    //    print("Distance" + Vector2.Distance(transform.position, enemyPrevPos));
    //    while (Vector2.Distance(transform.position, enemyPrevPos) > biteGrabArea)
    //    {
    //        var t = Mathf.InverseLerp(shortestDistance, biteGrabArea, Vector2.Distance(transform.position, enemyPrevPos));
    //        Mathf.Clamp(t, 0.1f, 1f);
    //        print("t" + t);
    //        mover.speed = jumpCurve.Evaluate(t) * jumpSpeed;
    //        mover.Move(movementDirection);
    //        print("Mover SPeed" + mover.speed);
    //        yield return null;
    //    }
    //    mover.speed = 1;
    //    biting = false;
    //    print("Exiting Jump Coroutine");
    //}

    #endregion

    public void DoDamage(uint strength)
    {
        ether = ether - strength;
    }

    public void ConsumeEther(uint cost)
    {
        DoDamage(cost);
    }
}
