using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3Script : AgentObject
{
    [SerializeField] float moveSpeed;
    [SerializeField] float slowingRadius;
    [SerializeField] float stoppingDistance;
    [SerializeField] float rotationSpeed;
    private Rigidbody2D rb;

    new void Start()
    {
        base.Start();
        Debug.Log("Arrival is imminent!");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Target == null)
        {
            Debug.LogWarning("No target assigned for arrival behavior!");
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

        Arrive(Target.position);
    }

    private void Arrive(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance <= stoppingDistance)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            float desiredSpeed;
            if (distance < slowingRadius)
            {
                desiredSpeed = moveSpeed * (distance / slowingRadius);
            }
            else
            {
                desiredSpeed = moveSpeed;
            }

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);
            float rotationStep = rotationSpeed * Time.deltaTime;
            float rotationAmount;

            if (Mathf.Abs(angleDifference) < rotationStep)
            {
                rotationAmount = angleDifference;
            }
            else if (angleDifference > 0)
            {
                rotationAmount = rotationStep;
            }
            else
            {
                rotationAmount = -rotationStep;
            }

            transform.Rotate(Vector3.forward, rotationAmount);

            rb.velocity = transform.up * desiredSpeed;
        }
    }
}
