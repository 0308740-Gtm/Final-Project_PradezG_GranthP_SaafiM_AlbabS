using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlotSelection : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Image[] slotBackgrounds = new Image[6];

    [SerializeField]
    private Color normalColor =
        new Color(0.18f, 0.15f, 0.10f, 1f);

    [SerializeField]
    private Color selectedColor =
        new Color(0.65f, 0.45f, 0.12f, 1f);

    public int SelectedSlot { get; private set; } = 1;

    private InputAction[] slotActions = new InputAction[6];

    private void Awake()
    {
        if (playerInput == null || playerInput.actions == null)
        {
            Debug.LogError("Assign PlayerInput in the Inspector.");
            enabled = false;
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            slotActions[i] = playerInput.actions.FindAction(
                "SelectSlot" + (i + 1), false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            if (slotActions[i] != null &&
                slotActions[i].WasPressedThisFrame())
            {
                SelectSlot(i + 1);
            }
        }
    }

    public void SelectSlot(int number)
    {
        if (number < 1 || number > 6)
            return;

        SelectedSlot = number;

        for (int i = 0; i < slotBackgrounds.Length; i++)
        {
            if (slotBackgrounds[i] != null)
            {
                slotBackgrounds[i].color =
                    i == number - 1 ? selectedColor : normalColor;
            }
        }
    }
}
