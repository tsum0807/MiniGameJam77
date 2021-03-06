using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    private PlayerController objController;
    private MonsterController monsterController;
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
        // if stuff are null
        if(objController == null)
        {
            monsterController = GetComponent<MonsterController>();
            prevDir = monsterController.facingDir;
            prevIsMoving = monsterController.isMoving;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInNewState())
        {
            PlayNewAnimation();
        }
        prevDir = GetDir();
        prevIsMoving = GetIsMoving();
        //prevDir = objController.facingDir;
        //prevIsMoving = objController.isMoving;
    }

    private int GetDir()
    {
        if(objController == null)
        {
            return monsterController.facingDir;
        }
        else
        {
            return objController.facingDir;
        }
    }

    private bool GetIsMoving()
    {
        return objController == null ? monsterController.isMoving : objController.isMoving;
    }

    private bool IsInNewState()
    {
        
        return (prevDir == GetDir() && prevIsMoving == GetIsMoving());
    }

    private void PlayNewAnimation()
    {
        int curDir = GetDir();
        bool curIsMoving = GetIsMoving();
        bool curIsFeared = false;
        if(objController != null)
            curIsFeared =  objController.isFeared;

        if (curIsFeared)
        {
            //AudioManager.AM.PlayScreamSound();
            //AudioManager.AM.PlayRunSound();
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
            //AudioManager.AM.PlayWalkSound();
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
