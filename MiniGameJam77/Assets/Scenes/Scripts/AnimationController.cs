using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    private PlayerController objController;
    private Animator animator;

    private int prevDir;
    private bool prevIsMoving;

    void Start()
    {
        animator = GetComponent<Animator>();
        //print(animator.GetClipCount());
        // get player, else get monster
        if((objController = GetComponent<PlayerController>()) == null)
        {
            //objController = GetComponent<MonsterController>();
        }
        prevDir = objController.facingDir;
        prevIsMoving = objController.isMoving;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInNewState())
        {
            PlayNewAnimation();
        }

        prevDir = objController.facingDir;
        prevIsMoving = objController.isMoving;
    }

    private bool IsInNewState()
    {
        return (prevDir == objController.facingDir && prevIsMoving == objController.isMoving);
    }

    private void PlayNewAnimation()
    {
        int curDir = objController.facingDir;
        bool curIsMoving = objController.isMoving;
        bool curIsFeared = objController.isFeared;

        if (curIsFeared)
        {
            // fear run
            switch (curDir)
            {
                case (0):
                    animator.Play("runDown");
                    break;
                case (1):
                    animator.Play("runLeft");
                    break;
                case (2):
                    animator.Play("runUp");
                    break;
                case (3):
                    animator.Play("runRight");
                    break;
            }
        }
        else if (curIsMoving)
        {
            // walk
            switch (curDir)
            {
                case (0):
                    animator.Play("walkDown");
                    break;
                case (1):
                    animator.Play("walkLeft");
                    break;
                case (2):
                    animator.Play("walkUp");
                    break;
                case (3):
                    animator.Play("walkRight");
                    break;
            }
        }
        else
        {
            // idle
            switch (curDir)
            {
                case (0):
                    animator.Play("idleDown");
                    break;
                case (1):
                    animator.Play("idleLeft");
                    break;
                case (2):
                    animator.Play("idleUp");
                    break;
                case (3):
                    animator.Play("idleRight");
                    break;
            }
                
        }
        //print(curDir + " not moving: " + curIsMoving);
    }
}
