using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    [SerializeField, Min(1)] private int amount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerVitals vitals =
            other.GetComponentInParent<PlayerVitals>();

        if (vitals == null)
            return;

        if (vitals.AddArrows(amount))
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
