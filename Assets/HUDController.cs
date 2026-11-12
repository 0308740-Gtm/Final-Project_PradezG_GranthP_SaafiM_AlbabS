using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerVitals playerVitals;

    [Header("Bars")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider ammoSlider;
    [SerializeField] private Slider arrowSlider;

    [Header("Labels")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text arrowText;

    private void OnEnable()
    {
        if (playerVitals != null)
            playerVitals.VitalsChanged += Refresh;
    }

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if (playerVitals != null)
            playerVitals.VitalsChanged -= Refresh;
    }

    private void Refresh()
    {
        if (playerVitals == null)
            return;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = playerVitals.MaxHealth;
            healthSlider.value = playerVitals.CurrentHealth;
        }

        if (ammoSlider != null)
        {
            ammoSlider.minValue = 0;
            ammoSlider.maxValue = playerVitals.MaxAmmo;
            ammoSlider.value = playerVitals.CurrentAmmo;
        }

        if (healthText != null)
        {
            healthText.text =
                $"HEALTH {playerVitals.CurrentHealth}/{playerVitals.MaxHealth}";
        }

        if (ammoText != null)
        {
            ammoText.text =
                $"AMMO {playerVitals.CurrentAmmo}/{playerVitals.MaxAmmo}";
        }

        if (arrowSlider != null)
        {
            arrowSlider.minValue = 0;
            arrowSlider.maxValue = playerVitals.MaxArrows;
            arrowSlider.value = playerVitals.CurrentArrows;
        }

        if (arrowText != null)
        {
            arrowText.text =
                $"ARROWS {playerVitals.CurrentArrows}/{playerVitals.MaxArrows}";
        }

        if (arrowSlider != null)
        {
            arrowSlider.maxValue = playerVitals.MaxArrows;
            arrowSlider.value = playerVitals.CurrentArrows;
        }

        if (arrowText != null)
        {
            arrowText.text =
                $"ARROWS {playerVitals.CurrentArrows}/{playerVitals.MaxArrows}";
        }

    }
}
