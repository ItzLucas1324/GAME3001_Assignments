using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimationController : MonoBehaviour
{
    private Animator an;

    void Start()
    {
        an = GetComponentInChildren<Animator>();
    }

    // Method to update the animation based on movement direction
    public void UpdateAnimation(Vector3 movementDirection)
    {
        // Debugging movement values
        Debug.Log("Movement Direction: " + movementDirection);
        Debug.Log("Movement Magnitude: " + movementDirection.magnitude);

        // If the agent is moving
        if (movementDirection.magnitude > 0.1f)
        {
            Debug.Log("Agent is moving");

            // Determine whether it's moving horizontally or vertically
            if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y)) // Horizontal movement
            {
                an.SetBool("isRunning", true);
                an.SetFloat("moveDirection", Mathf.Sign(movementDirection.x)); // Set move direction (1 for right, -1 for left)
                an.SetBool("isMovingVertically", false);
            }
            else // Vertical movement
            {
                an.SetBool("isRunning", true);
                an.SetFloat("moveDirectionY", Mathf.Sign(movementDirection.y)); // Set vertical move direction (1 for up, -1 for down)
                an.SetBool("isMovingVertically", true);
            }
        }
        else
        {
            // If the agent is not moving, set it to idle
            Debug.Log("Agent is idle");

            an.SetBool("isRunning", false);
            an.SetBool("isMovingVertically", false);
            an.SetFloat("moveDirection", 0f); // Reset horizontal direction
            an.SetFloat("moveDirectionY", 0f); // Reset vertical direction
        }
    }
}
