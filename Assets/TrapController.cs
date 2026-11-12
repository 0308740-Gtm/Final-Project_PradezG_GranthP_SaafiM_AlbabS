using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TrapController : MonoBehaviour
{
    public Transform spike;
    public Collider2D spikeCollider;

    public float riseDistance = 0.5f;
    public float riseSpeed = 3f;

    private Vector3 hiddenPosition;
    private Vector3 raisedPosition;

    private bool activated = false;
    private bool fullyRaised = false;

    void Start()
    {
        // Remember the initial position of the spike
        hiddenPosition = spike.localPosition;

        // Calculate where the spike should emerge
        raisedPosition = hiddenPosition +
                         Vector3.up * riseDistance;

        // Spike collision is initially disabled
        spikeCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Activate only when Player touches the plate
        if (other.CompareTag("Player"))
        {
            activated = true;
        }
    }

    void Update()
    {
        if (activated && !fullyRaised)
        {
            // Move the spike upwards
            spike.localPosition = Vector3.MoveTowards(
                spike.localPosition,
                raisedPosition,
                riseSpeed * Time.deltaTime
            );

            // Check whether the spike has fully emerged
            if (spike.localPosition == raisedPosition)
            {
                fullyRaised = true;

                // Now the spike can physically collide
                spikeCollider.enabled = true;
            }
        }
    }
}

