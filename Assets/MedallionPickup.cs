using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedallionPickup : MonoBehaviour
{
    [SerializeField] private MedallionType medallionType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory inventory =
            other.GetComponentInParent<PlayerInventory>();

        if (inventory == null)
            return;

        inventory.AddMedallion(medallionType);
        Destroy(gameObject);
    }

}
