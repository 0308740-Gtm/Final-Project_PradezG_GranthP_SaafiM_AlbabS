using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MedallionType
{
    Sun,
    Jaguar,
    Serpent
}

public class PlayerInventory : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("Medallions")]
    [SerializeField] private bool hasSunMedallion;
    [SerializeField] private bool hasJaguarMedallion;
    [SerializeField] private bool hasSerpentMedallion;

    [Header("Weapons")]
    [SerializeField] private bool hasLongGun;
    [SerializeField] private bool hasSword;
    [SerializeField] private bool hasCrossbow;

    public bool HasLongGun => hasLongGun;
    public bool HasSword => hasSword;
    public bool HasCrossbow => hasCrossbow;

    [Header("Treasure")]
    [SerializeField, Min(0)] private int goldNuggets;

    public event Action InventoryChanged;

    public int GoldNuggets => goldNuggets;

    public bool HasMedallion(MedallionType type)
    {
        switch (type)
        {
            case MedallionType.Sun:
                return hasSunMedallion;

            case MedallionType.Jaguar:
                return hasJaguarMedallion;

            case MedallionType.Serpent:
                return hasSerpentMedallion;

            default:
                return false;
        }
    }

    public bool AddMedallion(MedallionType type)
    {
        if (HasMedallion(type))
            return false;

        switch (type)
        {
            case MedallionType.Sun:
                hasSunMedallion = true;
                break;

            case MedallionType.Jaguar:
                hasJaguarMedallion = true;
                break;

            case MedallionType.Serpent:
                hasSerpentMedallion = true;
                break;

            default:
                return false;
        }

        InventoryChanged?.Invoke();
        return true;
    }

    public void AddGold(int amount)
    {
        if (amount <= 0)
            return;

        goldNuggets += amount;
        InventoryChanged?.Invoke();
    }

    public void AddLongGun()
    {
        if (hasLongGun)
            return;

        hasLongGun = true;
        InventoryChanged?.Invoke();
    }

    public void AddSword()
    {
        if (hasSword)
            return;

        hasSword = true;
        InventoryChanged?.Invoke();
    }

    public void AddCrossbow()
    {
        if (hasCrossbow)
            return;

        hasCrossbow = true;
        InventoryChanged?.Invoke();
    }
}
