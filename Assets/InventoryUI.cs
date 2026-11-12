using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("Inventory")]
    [SerializeField] private PlayerInventory inventory;

    [Header("Medallion Icons")]
    [SerializeField] private Image SunIcon;
    [SerializeField] private Image JaguarIcon;
    [SerializeField] private Image SerpentIcon;

    [Header("Weapon Icons")]
    [SerializeField] private Image LongGunIcon;
    [SerializeField] private Image SwordIcon;
    [SerializeField] private Image CrossbowIcon;

    [Header("Treasure")]
    [SerializeField] private TMP_Text goldText;

    private void OnEnable()
    {
        if (inventory != null)
            inventory.InventoryChanged += Refresh;

        Refresh();
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.InventoryChanged -= Refresh;
    }

    private void Refresh()
    {
        if (inventory == null)
            return;

        SetIcon(SunIcon,
            inventory.HasMedallion(MedallionType.Sun));

        SetIcon(JaguarIcon,
            inventory.HasMedallion(MedallionType.Jaguar));

        SetIcon(SerpentIcon,
            inventory.HasMedallion(MedallionType.Serpent));

        SetIcon(LongGunIcon, inventory.HasLongGun);
        SetIcon(SwordIcon, inventory.HasSword);
        SetIcon(CrossbowIcon, inventory.HasCrossbow);

        if (goldText != null)
            goldText.text = $"GOLD: {inventory.GoldNuggets}";
    }

    private void SetIcon(Image icon, bool owned)
    {
        if (icon == null)
            return;

        icon.enabled = true;

        icon.color = owned
            ? Color.white
            : new Color(0.35f, 0.35f, 0.35f, 1f);
    }
}
