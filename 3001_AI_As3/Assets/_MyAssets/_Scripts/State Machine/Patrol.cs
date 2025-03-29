using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;

    private Pathfinding pathfindingScript;
    private Transform target;

    bool hasReachedPatrolPoint = false;

    private void Awake()
    {
        pathfindingScript = FindObjectOfType<Pathfinding>();
        if (patrolPoints.Length == 0)
        {
            Debug.Log("No patrol points assigned");
        }
    }

    public void Patrolling()
    {
        if (patrolPoints.Length == 0) return;

        target = patrolPoints[currentPointIndex];
        pathfindingScript.target = target;

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            hasReachedPatrolPoint = true;
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
        else
        {
            hasReachedPatrolPoint = false;
        }
    }

    public bool HasReachedPatrolPoint()
    {
        return hasReachedPatrolPoint;
    }

    public bool IsAtPatrolPoint()
    {
        return Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position) < 1f;
    }
}
