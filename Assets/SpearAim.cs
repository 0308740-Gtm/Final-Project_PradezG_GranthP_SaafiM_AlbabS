using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class SpearAim : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position on the screen
            Vector3 mousePosition = Input.mousePosition;

            // Convert screen coordinates into world coordinates
            Vector3 targetPosition =
                Camera.main.ScreenToWorldPoint(mousePosition);

            // Calculate the direction from the spear to the target
            Vector2 direction =
                targetPosition - transform.position;

            // Calculate the angle in degrees
            float angle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the spear toward the target
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}*/


public class SpearAim : MonoBehaviour
{
    public float speed = 10f;

    private Vector3 targetPosition;
    private bool hasTarget = false;
    private bool isThrown = false;

    void Update()
    {
        // Once thrown, move independently
        if (isThrown)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            return;
        }

        // Aim at the last clicked screen position
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z =
                -Camera.main.transform.position.z;

            targetPosition =
                Camera.main.ScreenToWorldPoint(mousePosition);

            hasTarget = true;
            AimAtTarget();
        }

        // Throw the existing spear when X is pressed
        if (Input.GetKeyDown(KeyCode.X) && hasTarget)
        {
            ThrowSpear();
        }
    }

    void AimAtTarget() //It just aims, not throw
    {
        Vector2 direction =
            targetPosition - transform.position;

        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);
    }

    void ThrowSpear()
    {
        // Detach from Player while preserving world position
        transform.SetParent(null, true);

        isThrown = true;
    }
}
