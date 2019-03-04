using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAnimator(Vector3 velocity)
    {
        print(velocity);
        if (velocity.x > 0.001)
        {
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
        }
        else if (velocity.x < -0.001)
        {
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
        }
        else
        {
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
        }

        if (velocity.z > 0.001)
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", true);
        }
        else if (velocity.z < -0.001)
        {
            animator.SetBool("Front", true);
            animator.SetBool("Back", false);
        }
        else
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
        }
    }
}
