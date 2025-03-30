using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private SceneChanger scene;
    bool isIdle = false;

    [SerializeField] TMP_Text stateText;

    private void Awake()
    {
        patrolScript = GetComponent<Patrol>();
        controller = FindObjectOfType<AgentAnimationController>();
        chase = FindObjectOfType<Chase>();
        scene = FindObjectOfType<SceneChanger>();
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

    public void IdleState()
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
        ChangeStateText();
        isIdle = false;
    }

    public void PatrolState()
    {
        patrolScript.Patrolling();
        if (patrolScript.HasReachedPatrolPoint())  
        {
            currentState = State.Idle;
            ChangeStateText();
        }
    }

    public void ChaseState()
    {
        if (currentState != State.Chase)
        {
            currentState = State.Chase;
            chase.StartChase();
            ChangeStateText();
            Debug.Log("Switching to Chase state!");
        }
    }

    public void ChangeStateText()
    {
        if (currentState == State.Idle)
        {
            stateText.text = "Idle";
        }
        else if (currentState == State.Patrol)
        {
            stateText.text = "Patrol";
        }
        else
        {
            stateText.text = "Chase";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (scene == null)
        {
            Debug.LogError("Scene reference is null!");
            return;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Contact with Player");
            if (currentState == State.Chase)
            {
                scene.YouLose();
            }
            else if (currentState == State.Idle || currentState == State.Patrol)
            {
                scene.YouWin();
            }
        }
    }
}
