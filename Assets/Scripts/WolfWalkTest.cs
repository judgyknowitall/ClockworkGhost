using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfWalkTest : MonoBehaviour
{
    public Animator animator;
    public Mover mover;
	
	void Update ()
    {
        var direction = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            mover.speed = 2;
            animator.SetFloat("AnimMultiplier", 2);
        }
        else
        {
            mover.speed = 1;
            animator.SetFloat("AnimMultiplier", 1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector2.left;
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
        }
        else
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }

        mover.Move(direction);
    }
}
