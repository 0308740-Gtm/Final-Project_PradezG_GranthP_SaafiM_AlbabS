using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float x = 0f;
        float y = 0f;

        if (Input.GetKey(KeyCode.A))
            x = -1f;

        if (Input.GetKey(KeyCode.D))
            x = 1f;

        if (Input.GetKey(KeyCode.W))
            y = 1f;

        if (Input.GetKey(KeyCode.S))
            y = -1f;

        Vector3 movement = new Vector3(x, y, 0f);

        transform.position += movement * speed * Time.deltaTime;
    }
}
*/


public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;

    private Rigidbody2D rb;

    private float horizontalInput;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
     
    }

    void Update()
    {
        // Read horizontal movement
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;

        if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        // Jump only when standing on the platform
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(
                rb.velocity.x,
                jumpForce
            );

            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement using physics
        rb.velocity = new Vector2(
            horizontalInput * speed,
            rb.velocity.y
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}