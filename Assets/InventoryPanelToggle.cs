using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryPanel == null || Keyboard.current == null)
            return;

        bool togglePressed =
            Keyboard.current.iKey.wasPressedThisFrame ||
            Keyboard.current.tabKey.wasPressedThisFrame;

        if (togglePressed)
        {
            inventoryPanel.SetActive(
                !inventoryPanel.activeSelf);
        }
    }
}
