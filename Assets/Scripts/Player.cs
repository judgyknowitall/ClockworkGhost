﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Player : MonoBehaviour, IDamageable
{
    private Mover mover;
    [SerializeField] private Animator[] animators;

    [SerializeField] private GameObject shadow;

    public float ether;

    [SerializeField]
    private Head head;

    private bool biting = false;

    [SerializeField] private float biteCircleWidth = 0.1f;
    [SerializeField] private float jumpPauseTime;
    [SerializeField] private int jumpTimeSteps;
    [SerializeField] private float biteCircleMaxDistance;
    [SerializeField] private float etherFromBite;

    void Start ()
    {
        mover = GetComponent<Mover>();
    }

    void FixedUpdate ()
    {
        Move();
        UseAbilities();
	}

    #region Abilities
    void UseAbilities()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Bats();
            ConsumeEther(50);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Ability 2!");
            ConsumeEther(75);
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
        foreach (Animator animator in animators)
        {
            shadow.SetActive(false);
            animator.SetBool("Bat", true);
        }
        shadow.SetActive(true);
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


    private void BiteAttempt()
    {
        print("Attempting Bite");
        Debug.DrawRay(transform.position, head.direction.normalized * biteCircleMaxDistance, Color.red, 5f);
        RaycastHit2D[] spherecastResults = Physics2D.CircleCastAll(transform.position, biteCircleWidth, head.direction, biteCircleMaxDistance);
        RaycastHit2D enemyToHit = new RaycastHit2D();
        bool enemyFound = false;
        foreach (RaycastHit2D hit in spherecastResults)
        {
            if (hit.collider == null || hit.collider.gameObject.GetComponent<Enemy>() == null )//|| !hit.collider.gameObject.GetComponent<Enemy>().stunned)
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
        mover.displayOrderOffset += 1;
        spherecastResult.collider.gameObject.GetComponent<Enemy>().beingKilled = true;

        Vector2 enemyPosition = spherecastResult.transform.position;
        Vector2 playerPosition = transform.position;

        IEnumerator lungeAnimation = Jump(enemyPosition, playerPosition, spherecastResult);
        print("Begining Jump");
        StartCoroutine(lungeAnimation);
    }

    private IEnumerator Jump(Vector2 enemyPosition, Vector2 playerPosition, RaycastHit2D spherecastResult)
    {
        print("Breif Pause");
        foreach (Animator animator in animators)
        {
            animator.SetBool("RedFlash", true);
        }
        for (int i = 0; i < jumpPauseTime; i++)
        {
            yield return new WaitForSeconds(1); 
        }
        print("Done Pause");

        float distancePerFrame = Vector2.Distance(enemyPosition, playerPosition) / jumpTimeSteps;
        Vector2 direction = (enemyPosition - playerPosition).normalized;

        foreach (Animator animator in animators)
        {
            animator.SetBool("RedFlash", false);
        }
        foreach (Animator animator in animators)
        {
            animator.SetBool("RedFlashIdle", true);
        }

        print("Jumping");
        for (int i = 0; i < jumpTimeSteps; i++)
        {
            print("Jump step" + i);
            transform.position = playerPosition + (direction * (distancePerFrame * i));
            yield return null;
        }

        print("Jump Complete");
        FinishBite(spherecastResult);
    }

    private void FinishBite(RaycastHit2D spherecastResult)
    {
        spherecastResult.collider.gameObject.GetComponent<Enemy>().stunned=true;
        Destroy(spherecastResult.collider.gameObject);
        ether += etherFromBite;
        biting = false;
        foreach (Animator animator in animators)
        {
            animator.SetBool("RedFlashIdle", false);
        }
        mover.displayOrderOffset -= 1;
        print("Finished Bite");
    }

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
