using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player4Script : AgentObject
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float detectionRadius;
    [SerializeField] float avoidanceStrength;
    [SerializeField] float stoppingDistance;
    private Rigidbody2D rb;

    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Target == null)
        {
            Debug.LogWarning("No target assigned!");
            return;
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

        AvoidAndSeek(Target.position);
    }

    private void AvoidAndSeek(Vector3 targetPosition)
    {
        Vector2 directionToTarget = (targetPosition - transform.position).normalized;

        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position,
            detectionRadius,
            directionToTarget,
            Mathf.Infinity,
            LayerMask.GetMask("Hazard")
        );

        Vector2 avoidanceVector = Vector2.zero;

        if (hit.collider != null)
        {
            Vector2 hazardDirection = (transform.position - hit.collider.transform.position).normalized;

            float proximityFactor = Mathf.Clamp01(1 - hit.distance / detectionRadius);
            avoidanceVector = hazardDirection * (avoidanceStrength * proximityFactor);
        }
        else
        {
            Debug.Log("No hazard detected.");
        }

        Vector2 combinedDirection = directionToTarget + avoidanceVector;
        combinedDirection.Normalize();

        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        float currentSpeed;
        if (distanceToTarget <= stoppingDistance)
        {
            currentSpeed = 0f;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        float targetAngle = Mathf.Atan2(combinedDirection.y, combinedDirection.x) * Mathf.Rad2Deg - 90.0f;
        float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);

        float rotationStep = rotationSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
        transform.Rotate(Vector3.forward, rotationAmount);

        rb.velocity = transform.up * currentSpeed;

        if (distanceToTarget <= stoppingDistance)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
