using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player4Script : AgentObject
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float whiskerLength;
    [SerializeField] float whiskerAngle;
    [SerializeField] float avoidanceWeight;
    [SerializeField] LayerMask whiskerLayer;
    [SerializeField] AudioClip clip;
    private AudioSource aud;
    private Rigidbody2D rb;

    new void Start()
    {
        base.Start();
        Debug.Log("Avoiding the enemy!");
        aud = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Target == null)
        {
            Debug.LogWarning("No target assigned!");
            return;
        }

        if (Target != null)
        {
            SeekForward(Target.position);
            AvoidAndSeek();

            if (Vector2.Distance(transform.position, Target.position) <= 0.1f)
            {
                rb.velocity = Vector2.zero;
                return;
            }
        }

        Vector3 offset = gameObject.transform.position;
        if (offset.x < -7f)
        {
            offset.x = -7f;
        }
        else if (offset.x > 7f)
        {
            offset.x = 7f;
        }
        if (offset.y < -4f)
        {
            offset.y = -4f;
        }
        else if (offset.y > 4f)
        {
            offset.y = 4f;
        }
        transform.position = offset;
    }


    private void AvoidAndSeek()
    {
        bool hitRight = CastWhisker(whiskerAngle, Color.red);
        bool hitLeft = CastWhisker(-whiskerAngle, Color.blue);

        if (hitRight)
        {
            RotateClockwise();
        }
        else if (hitLeft)
        {
            RotateCounterClockwise();
        }
    }

    private void RotateCounterClockwise()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * avoidanceWeight * Time.deltaTime);
    }

    private void RotateClockwise()
    {
        transform.Rotate(Vector3.forward, -rotationSpeed * avoidanceWeight * Time.deltaTime);
    }

    private bool CastWhisker(float angle, Color color)
    {
        bool hitResult = false;
        Color rayColor = color;

        Vector2 whiskerDirection = Quaternion.Euler(0, 0, angle) * transform.up;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, whiskerDirection, whiskerLength, whiskerLayer);

        if (hit.collider != null)
        {
            Debug.Log("Obastacle detected!");
            rayColor = Color.green;
            hitResult = true;
        }

        Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, rayColor);

        return hitResult;
    }
        private void SeekForward(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f;

        float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z);
        float rotationStep = rotationSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
        transform.Rotate(Vector3.forward, rotationAmount);

        rb.velocity = transform.up * moveSpeed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrive"))
        {
            aud.PlayOneShot(clip);
        }
    }
}
