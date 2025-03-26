using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Rigidbody2D rb;
    private float lastHorizontalInput = 0;
    private float lastVerticalInput = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0)
        {
            lastHorizontalInput = moveX;
            lastVerticalInput = 0;
        }

        if (moveY != 0)
        {
            lastVerticalInput = moveY;
            lastHorizontalInput = 0;
        }

        if (moveX == 0 && moveY ==0)
        {
            lastHorizontalInput = 0;
            lastVerticalInput = 0;
        }

        Vector2 moveDirection = new Vector2(lastHorizontalInput, lastVerticalInput).normalized;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
