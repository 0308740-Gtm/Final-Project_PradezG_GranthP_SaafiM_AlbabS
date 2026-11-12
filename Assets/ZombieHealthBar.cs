using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ZombieHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ZombieHealth zombieHealth;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        if (zombieHealth == null)
            zombieHealth = GetComponentInParent<ZombieHealth>();

        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        if (zombieHealth != null)
            zombieHealth.HealthChanged += UpdateHealthBar;
    }

    private void Start()
    {
        if (zombieHealth == null || healthSlider == null)
        {
            Debug.LogError(
                "ZombieHealthBar: Missing references.", this);
            return;
        }

        UpdateHealthBar(
            zombieHealth.CurrentHealth,
            zombieHealth.MaxHealth
        );
    }

    private void OnDisable()
    {
        if (zombieHealth != null)
            zombieHealth.HealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthSlider == null)
            return;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            healthSlider.gameObject.SetActive(false);
    }


}
