using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class MedallionGate : MonoBehaviour
{
    [Header("Requirement")]
    [SerializeField] private MedallionType requiredMedallion;

    [Header("Gate Parts")]
    [SerializeField] private Transform gateVisual;
    [SerializeField] private Collider2D solidCollider;

    [SerializeField]
    private Vector3 openOffset = new Vector3(1.5f, 0f, 0f);

    [SerializeField, Min(0.1f)]
    private float openDuration = 0.8f;

    [Header("Interaction Prompt")]
    [SerializeField] private TMP_Text promptText;

    private PlayerInventory nearbyInventory;
    private bool isOpening;
    private bool isOpen;


    // Start is called before the first frame update
    void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (nearbyInventory == null || isOpening || isOpen)
            return;

        RefreshPrompt();

        if (Keyboard.current == null ||
            !Keyboard.current.spaceKey.wasPressedThisFrame)
            return;

        if (nearbyInventory.HasMedallion(requiredMedallion))
        {
            StartCoroutine(OpenGateRoutine());
        }
        else
        {
            Debug.Log(
                $"Requires the {requiredMedallion} Medallion.");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory inventory =
            other.GetComponentInParent<PlayerInventory>();

        if (inventory == null)
            return;

        nearbyInventory = inventory;

        if (promptText != null)
            promptText.gameObject.SetActive(true);

        RefreshPrompt();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerInventory inventory =
            other.GetComponentInParent<PlayerInventory>();

        if (inventory != nearbyInventory)
            return;

        nearbyInventory = null;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void RefreshPrompt()
    {
        if (promptText == null ||
            nearbyInventory == null ||
            isOpen)
            return;

        if (nearbyInventory.HasMedallion(requiredMedallion))
        {
            promptText.text =
                $"PRESS SPACE — UNLOCK {requiredMedallion.ToString().ToUpper()} GATE";
        }
        else
        {
            promptText.text =
                $"SEALED — REQUIRES {requiredMedallion.ToString().ToUpper()} MEDALLION";
        }
    }

    private IEnumerator OpenGateRoutine()
    {
        isOpening = true;

        if (promptText != null)
            promptText.text = "ANCIENT STONE GRINDS OPEN...";

        if (gateVisual != null)
        {
            Vector3 startPosition = gateVisual.localPosition;
            Vector3 targetPosition = startPosition + openOffset;
            float elapsed = 0f;

            while (elapsed < openDuration)
            {
                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(elapsed / openDuration);

                gateVisual.localPosition =
                    Vector3.Lerp(startPosition, targetPosition, t);

                yield return null;
            }

            gateVisual.localPosition = targetPosition;
        }

        if (solidCollider != null)
            solidCollider.enabled = false;

        isOpen = true;
        isOpening = false;

        if (promptText != null)
            promptText.text = "GATE UNLOCKED";
    }

}
