using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform island;
    [SerializeField] private float loadDistance;
    private Vector3 targetPosition = Vector3.zero;

    void Update()
    {
        // Move towards the target position.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotate to look at the target position.
        LookAt2D(targetPosition);
    }

    void LookAt2D(Vector3 target)
    {
        Vector3 lookDirection = target - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

