using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : AgentObject
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    private Rigidbody2D rb;

    new void Start()
    {
        base.Start();
        Debug.Log("Starting figher...");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Target != null)
        {
            SeekForward(Target.position);
        }
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
}
