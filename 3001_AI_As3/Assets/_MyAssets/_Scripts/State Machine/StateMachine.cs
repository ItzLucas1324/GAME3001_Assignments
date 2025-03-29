using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase
    }

    public State currentState;
    private Patrol patrolScript;
    bool isIdle = false;

    private void Awake()
    {
        patrolScript = GetComponent<Patrol>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Patrol:
                PatrolState();
                break;
            case State.Chase:
                ChaseState();
                break;
        }
    }

    private void IdleState()
    {
        if (!isIdle)
        {
            isIdle = true;
            Debug.Log("Entering Idle state");

            StartCoroutine(IdleTimer());
        }
    }

    private IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("Exiting Idle state, returning to Patrol");
        currentState = State.Patrol;
        isIdle = false;
    }

    private void PatrolState()
    {
        patrolScript.Patrolling();
        if (patrolScript.HasReachedPatrolPoint())  
        {
            Debug.Log("Reached Patrol Point, going Idle");
            currentState = State.Idle;
        }
    }

    private void ChaseState()
    {
        // Logic for chase state (not implemented in this example)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Patrol Point"))
        {
            Debug.Log("Entering Idle State");
            currentState = State.Idle;
        }
    }
}
