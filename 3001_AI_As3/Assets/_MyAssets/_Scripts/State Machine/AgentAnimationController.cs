using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimationController : MonoBehaviour
{
    private Animator an;
    private int lastMoveX = 0;
    private int lastMoveY = 0;
    private bool isInIdleState = true;

    void Start()
    {
        an = GetComponentInChildren<Animator>();

        if (an == null)
        {
            Debug.LogError("Animator component not found in children!");
            return;
        }

        SetIdleState();
    }

    public void UpdateAnimation(int movementX, int movementY)
    {
        if (an == null) return;

        if (movementX == lastMoveX && movementY == lastMoveY &&
            (movementX != 0 || movementY != 0 || isInIdleState))
        {
            return;
        }

        lastMoveX = movementX;
        lastMoveY = movementY;

        bool isMoving = (movementX != 0 || movementY != 0);

        if (isMoving)
        {
            Debug.Log($"Agent MOVING: X={movementX}, Y={movementY}");
            isInIdleState = false;

            an.SetBool("isRunning", true);

            if (movementY != 0)
            {
                an.SetBool("isMovingVertically", true);
                an.SetFloat("moveDirectionY", movementY);
                an.SetFloat("moveDirection", 0);
            }
            else
            {
                an.SetBool("isMovingVertically", false);
                an.SetFloat("moveDirection", movementX);
                an.SetFloat("moveDirectionY", 0);
            }
        }
    }

    public void SetIdleState()
    {
        if (an == null) return;

        an.SetBool("isRunning", false);
        an.SetFloat("moveDirection", 0);
        an.SetFloat("moveDirectionY", 0);
        an.SetBool("isMovingVertically", false);

        isInIdleState = true;
        lastMoveX = 0;
        lastMoveY = 0;
    }
}