using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] float whiskerLength;
    [SerializeField] float whiskerAngle;
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float idleTimeout = 1f;
    private bool isDetected = false;
    private float detectionTimer = 0f;

    private StateMachine stateMachine;
    private Pathfinding pathfindingScript;


    private void Awake()
    {
        stateMachine = FindObjectOfType<StateMachine>();
        pathfindingScript = FindObjectOfType<Pathfinding>();
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        bool playerDetectedRight = CastWhisker(whiskerAngle, Color.red);
        bool playerDetectedLeft = CastWhisker(-whiskerAngle, Color.red);
        bool playerDetectedFarRight = CastWhisker(whiskerAngle * 2, Color.red);
        bool playerDetectedFarLeft = CastWhisker(-whiskerAngle * 2, Color.red);
        bool playerDetectedFarStraight = CastWhisker(0, Color.red);

        bool isPlayerDetected = playerDetectedRight || playerDetectedLeft || playerDetectedFarRight || playerDetectedFarLeft || playerDetectedFarStraight;


        if (isPlayerDetected)
        {
            if (!isDetected)
            {
                isDetected = true;
                detectionTimer = 0f;
                stateMachine.ChaseState();
            }
        }
        else
        {
            if (isDetected)
            {
                detectionTimer += Time.deltaTime;

                if (detectionTimer >= idleTimeout)
                {
                    isDetected = false;
                    pathfindingScript.target = null;
                    stateMachine.IdleState();
                }
            }
        }
    }

    private bool CastWhisker(float angle, Color rayColor)
    {
        Vector2 whiskerDirection = Quaternion.Euler(0, 0, angle) * transform.up;

        RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, whiskerDirection, whiskerLength, obstacleLayer);

        // If we hit an obstacle, stop and don't check for the player
        if (obstacleHit.collider != null)
        {
            Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, Color.yellow);
            return false;
        }

        // Cast another ray to detect the player if no obstacle is hit
        RaycastHit2D playerHit = Physics2D.Raycast(transform.position, whiskerDirection, whiskerLength, detectionLayer);

        if (playerHit.collider != null && playerHit.collider.CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, Color.green);
            return true;
        }

        Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, rayColor);
        return false;
    }
}