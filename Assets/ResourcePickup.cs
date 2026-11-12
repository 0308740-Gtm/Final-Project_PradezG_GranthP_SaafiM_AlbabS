using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    public enum ResourcePickupType
    {
        Health,
        Ammo,
        Gold
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] private ResourcePickupType pickupType;

    [SerializeField, Min(1)]
    private int amount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerVitals vitals =
            other.GetComponentInParent<PlayerVitals>();

        PlayerInventory inventory =
            other.GetComponentInParent<PlayerInventory>();

        bool collected = false;

        switch (pickupType)
        {
            case ResourcePickupType.Health:
                if (vitals != null &&
                    !vitals.IsDead &&
                    vitals.CurrentHealth < vitals.MaxHealth)
                {
                    vitals.Heal(amount);
                    collected = true;
                }
                break;

            case ResourcePickupType.Ammo:
                if (vitals != null &&
                    !vitals.IsDead &&
                    vitals.CurrentAmmo < vitals.MaxAmmo)
                {
                    vitals.AddAmmo(amount);
                    collected = true;
                }
                break;

            case ResourcePickupType.Gold:
                if (inventory != null)
                {
                    inventory.AddGold(amount);
                    collected = true;
                }
                break;
        }

        if (collected)
            Destroy(gameObject);
    }
}
