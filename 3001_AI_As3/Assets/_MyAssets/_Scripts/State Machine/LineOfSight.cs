using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] float whiskerLength; 
    [SerializeField] float whiskerAngle; 
    [SerializeField] LayerMask detectionLayer; 

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
            Debug.Log("Player detected! Switching to Chase state.");
            // Add your state switch logic here, for example:
            // ChangeState(Chase);
        }
    }

    private bool CastWhisker(float angle, Color rayColor)
    {
        Vector2 whiskerDirection = Quaternion.Euler(0, 0, angle) * transform.up;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, whiskerDirection, whiskerLength, detectionLayer);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                rayColor = Color.green;
                Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, rayColor);
                return true; 
            }
        }

        Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, rayColor);
        return false;
    }
}