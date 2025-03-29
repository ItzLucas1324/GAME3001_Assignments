using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Patrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;

    private Pathfinding pathfindingScript;
    private Transform target;

    [SerializeField] TMP_Text timerText;
    float patrolTimer = 0f;
    bool isTiming = false;

    bool hasReachedPatrolPoint = false;

    private void Awake()
    {
        pathfindingScript = FindObjectOfType<Pathfinding>();
        if (patrolPoints.Length == 0)
        {
            Debug.Log("No patrol points assigned");
        }

        if (timerText != null)
        {
            timerText.text = "";
        }
    }

    public void Patrolling()
    {
        if (patrolPoints.Length == 0) return;

        target = patrolPoints[currentPointIndex];
        pathfindingScript.target = target;

        if (!isTiming)
        {
            StartPatrolTimer();
        }

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            hasReachedPatrolPoint = true;
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            StopPatrolTimer();
        }
        else
        {
            hasReachedPatrolPoint = false;
            UpdatePatrolTimer();
        }
    }

    private void StartPatrolTimer()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        patrolTimer = (distance / pathfindingScript.moveSpeed) * 1.1f;
        isTiming = true;
    }

    private void StopPatrolTimer()
    {
        isTiming = false;
        patrolTimer = 0;
        timerText.text = "0";
    }

    private void UpdatePatrolTimer()
    {
        if (!isTiming) return;

        patrolTimer -= Time.deltaTime;

        int displayedTime = Mathf.FloorToInt(patrolTimer) + 1;

        if (patrolTimer <= 0)
        {
            StopPatrolTimer();
            return;
        }

        timerText.text = displayedTime.ToString();
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
