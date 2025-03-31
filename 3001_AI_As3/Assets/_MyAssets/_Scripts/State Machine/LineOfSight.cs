using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] float whiskerLength;
    [SerializeField] float whiskerAngle;
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float idleTimeout;
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

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, whiskerDirection, whiskerLength, detectionLayer | obstacleLayer);

        // Sort hits by distance
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit2D hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
            {
                Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, Color.yellow);
                return false;
            }

            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, Color.green);
                return true;
            }
        }

        Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, rayColor);
        return false;
    }
}