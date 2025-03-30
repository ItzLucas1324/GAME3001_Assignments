using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Rigidbody2D rb;
    private float lastHorizontalInput = 0;
    private float lastVerticalInput = 0;
    private Animator an;
    string currentScene;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        an = GetComponentInChildren<Animator>();

        currentScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        GameManager.Instance.SoundManager.StopLoopingSound();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (currentScene != "Play Scene") 
        {
            GameManager.Instance.SoundManager.StopLoopingSound();
            return;
        }

        if (moveX != 0 || moveY != 0)
        {
            GameManager.Instance.SoundManager.PlayLoopingSound("Player Walking");
        }
        else
        {
            GameManager.Instance.SoundManager.StopLoopingSound();
        }

        if (moveX != 0)
        {
            lastHorizontalInput = moveX;
            lastVerticalInput = 0;

            an.SetBool("isRunning", true);
            an.SetFloat("moveDirection", moveX);
            an.SetBool("isMovingVertically", false);
        }

        if (moveY != 0)
        {
            lastVerticalInput = moveY;
            lastHorizontalInput = 0;
            an.SetBool("isMovingVertically", true);
            an.SetFloat("moveDirectionY", moveY);
            an.SetBool("isRunning", true);
        }

        if (moveX == 0 && moveY == 0)
        {
            lastHorizontalInput = 0;
            lastVerticalInput = 0;
            an.SetBool("isRunning", false);
            an.SetFloat("moveDirection", moveX);
            an.SetBool("isMovingVertically", false);
            an.SetFloat("moveDirectionY", moveY);
        }

        Vector2 moveDirection = new Vector2(lastHorizontalInput, lastVerticalInput).normalized;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            GameManager.Instance.SoundManager.PlaySound("Wall Bump", 1f);
        }
    }
}
