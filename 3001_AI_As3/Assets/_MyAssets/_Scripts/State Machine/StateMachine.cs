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
    private AgentAnimationController controller;
    private Chase chase;
    bool isIdle = false;

    private void Awake()
    {
        patrolScript = GetComponent<Patrol>();
        controller = FindObjectOfType<AgentAnimationController>();
        chase = FindObjectOfType<Chase>();
    }

    private void Start()
    {
        currentState = State.Idle; // Set to Idle at the start
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

            controller.SetIdleState();

            StartCoroutine(IdleTimer());
        }
    }

    private IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(2f);

        currentState = State.Patrol;
        isIdle = false;
    }

    private void PatrolState()
    {
        patrolScript.Patrolling();
        if (patrolScript.HasReachedPatrolPoint())  
        {
            currentState = State.Idle;
        }
    }

    public void ChaseState()
    {
        if (currentState != State.Chase)
        {
            currentState = State.Chase;
            chase.StartChase();
            Debug.Log("Switching to Chase state!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Patrol Point"))
        {
            currentState = State.Idle;
        }
    }
}
