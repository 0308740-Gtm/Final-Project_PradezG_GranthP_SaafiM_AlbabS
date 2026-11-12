using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PLAYER : MonoBehaviour
{
    public float movespeed = 5f;
    public bool canMove = true;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;

    public static string spawnPointName;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (!string.IsNullOrEmpty(spawnPointName))
        {
            GameObject spawnPoint = GameObject.Find(spawnPointName);
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }
            spawnPointName = null;
        }
    }

    void OnMove(InputValue value)
    {
        if (!canMove) return;

        moveInput = value.Get<Vector2>();

        if (anim != null)
        {
            anim.SetFloat("MovementX", moveInput.x);
            anim.SetFloat("MovementY", moveInput.y);
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            rb.velocity = Vector2.zero;

            if (anim != null)
            {
                anim.SetFloat("MovementX", 0f);
                anim.SetFloat("MovementY", 0f);
            }
            return;
        }

        rb.velocity = moveInput * movespeed;
    }
}