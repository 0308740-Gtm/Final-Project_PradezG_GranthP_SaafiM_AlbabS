using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private QuestTracker questTracker;

    void Start()
    {
        questTracker = FindObjectOfType<QuestTracker>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (questTracker != null)
            {
                questTracker.hasKey = true;
                Destroy(gameObject);
            }
        }
    }
}
