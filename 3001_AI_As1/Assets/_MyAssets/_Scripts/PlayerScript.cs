using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : AgentObject
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] AudioClip clip;
    private AudioSource aud;
    private Rigidbody2D rb;

    new void Start()
    {
        base.Start();
        Debug.Log("Beginning the chase!");
        aud = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Target != null)
        {
            SeekForward(Target.position);
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
        if (collision.CompareTag("Seek"))
        {
            aud.PlayOneShot(clip);
        }
    }
}