using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Script : AgentObject
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    private Rigidbody2D rb;

    new void Start()
    {
        base.Start();
        Debug.Log("Starting to run away!");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Target != null)
        {
            Flee(Target.position);
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

    private void Flee(Vector3 targetPosition)
    {
        Vector2 direction = (transform.position - targetPosition).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f;

        float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z);
        float rotationStep = rotationSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
        transform.Rotate(Vector3.forward, rotationAmount);

        rb.velocity = transform.up * moveSpeed;
    }
}