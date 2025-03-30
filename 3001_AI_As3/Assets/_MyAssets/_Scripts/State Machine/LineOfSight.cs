using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] float whiskerLength; 
    [SerializeField] float whiskerAngle; 
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] LayerMask obstacleLayer;

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

        if (playerDetectedRight || playerDetectedLeft || playerDetectedFarRight || playerDetectedFarLeft || playerDetectedFarStraight)
        {
            FindObjectOfType<StateMachine>().ChaseState();
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