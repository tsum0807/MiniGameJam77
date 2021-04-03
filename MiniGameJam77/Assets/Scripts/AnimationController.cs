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
        print("changing anim");

        if (curIsMoving)
        {
            // run
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
            }

        }
    }
}
