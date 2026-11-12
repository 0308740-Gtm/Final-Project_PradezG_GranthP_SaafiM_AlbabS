using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("Health")]
    [SerializeField, Min(1)] private int maxHealth = 100;
    [SerializeField, Min(0)] private int currentHealth = 100;

    [Header("Ammo")]
    [SerializeField, Min(1)] private int maxAmmo = 100;
    [SerializeField, Min(0)] private int currentAmmo = 30;


    [Header("Arrows")]
    [SerializeField, Min(1)] private int maxArrows = 30;
    [SerializeField, Min(0)] private int currentArrows = 10;

    public int MaxArrows => maxArrows;
    public int CurrentArrows => currentArrows;


    public System.Action VitalsChanged;
    public System.Action PlayerDied;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int MaxAmmo => maxAmmo;
    public int CurrentAmmo => currentAmmo;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        maxAmmo = Mathf.Max(1, maxAmmo);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);

        currentArrows = Mathf.Clamp(currentArrows, 0, maxArrows);

        IsDead = currentHealth <= 0;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        VitalsChanged?.Invoke();

        if (currentHealth == 0)
        {
            IsDead = true;
            PlayerDied?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        VitalsChanged?.Invoke();
    }

    public bool TrySpendAmmo(int amount = 1)
    {
        if (IsDead || amount <= 0 || currentAmmo < amount)
            return false;

        currentAmmo -= amount;
        VitalsChanged?.Invoke();
        return true;
    }

    public void AddAmmo(int amount)
    {
        if (IsDead || amount <= 0)
            return;

        currentAmmo = Mathf.Min(maxAmmo, currentAmmo + amount);
        VitalsChanged?.Invoke();
    }

    public bool AddArrows(int amount)
    {
        if (IsDead || amount <= 0 || currentArrows >= maxArrows)
            return false;

        int newAmount = Mathf.Min(
            maxArrows, currentArrows + amount);

        if (newAmount == currentArrows)
            return false;

        currentArrows = newAmount;
        VitalsChanged?.Invoke();
        return true;
    }

    public bool TrySpendArrows(int amount = 1)
    {
        if (IsDead || amount <= 0 || currentArrows < amount)
            return false;

        currentArrows -= amount;
        VitalsChanged?.Invoke();
        return true;
    }
}
