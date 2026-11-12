using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        if (bossHealth != null)
            bossHealth.HealthChanged += UpdateBar;
    }

    private void Start()
    {
        if (bossHealth == null || healthSlider == null)
        {
            Debug.LogError(
                "BossHealthBar references are missing!", this
            );
            return;
        }

        healthSlider.minValue = 0;
        healthSlider.maxValue = bossHealth.MaxHealth;

        UpdateBar(
            bossHealth.CurrentHealth,
            bossHealth.MaxHealth
        );
    }

    private void OnDisable()
    {
        if (bossHealth != null)
            bossHealth.HealthChanged -= UpdateBar;
    }

    private void UpdateBar(int current, int maximum)
    {
        if (healthSlider == null)
            return;

        healthSlider.maxValue = maximum;
        healthSlider.value = current;
    }
}
