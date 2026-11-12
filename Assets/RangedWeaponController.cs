using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeaponController : MonoBehaviour
{
    private enum RangedWeapon
    {
        None,
        LongGun,
        Crossbow
    }

    [Header("Player References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerVitals vitals;
    [SerializeField] private SwordController swordController;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Character")]
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Animator characterAnimator;

    [Header("Held Sprites")]
    [SerializeField] private Sprite longGunHeldSprite;
    [SerializeField] private Sprite crossbowHeldSprite;

    [Header("Projectile Prefabs")]
    [SerializeField] private Projectile bulletPrefab;
    [SerializeField] private Projectile arrowPrefab;

    [Header("Long Gun")]
    [SerializeField, Min(0.1f)] private float bulletSpeed = 12f;
    [SerializeField, Min(1)] private int bulletDamage = 20;
    [SerializeField, Min(0.05f)] private float gunCooldown = 0.35f;

    [Header("Crossbow")]
    [SerializeField, Min(0.1f)] private float arrowSpeed = 9f;
    [SerializeField, Min(1)] private int arrowDamage = 35;
    [SerializeField, Min(0.05f)] private float crossbowCooldown = 0.6f;

    [Header("Input Actions")]
    [SerializeField] private string longGunActionName = "SelectSlot5";
    [SerializeField] private string crossbowActionName = "SelectSlot6";
    [SerializeField] private string fireActionName = "Fire";
    [SerializeField] private string moveActionName = "Move";

    private InputAction longGunAction;
    private InputAction crossbowAction;
    private InputAction fireAction;
    private InputAction moveAction;

    private RangedWeapon equippedWeapon = RangedWeapon.None;

    private Vector2 facingDirection = Vector2.right;
    private float nextFireTime;

    private Sprite originalSprite;
    private bool originalFlipX;
    private bool originalAnimatorEnabled;
    private bool visualsOverridden;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip crossbowFireClip;
    public AudioClip musketFireClip;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (vitals == null)
            vitals = GetComponent<PlayerVitals>();

        if (swordController == null)
            swordController = GetComponent<SwordController>();

        if (characterRenderer == null)
            characterRenderer = GetComponentInChildren<SpriteRenderer>();

        if (characterAnimator == null)
            characterAnimator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        BindInputActions();
    }

    private void OnDisable()
    {
        UnbindInputActions();
        UnequipForWeaponSwitch();
    }

    private void Update()
    {
        if (moveAction != null && moveAction.enabled)
        {
            Vector2 movement = moveAction.ReadValue<Vector2>();

            if (movement.sqrMagnitude > 0.01f)
                facingDirection = movement.normalized;
        }

        if (equippedWeapon != RangedWeapon.None &&
            characterRenderer != null)
        {
            characterRenderer.flipX = facingDirection.x < 0f;
        }
    }
    private void BindInputActions()
    {
        if (playerInput == null || playerInput.actions == null)
        {
            Debug.LogError(
                "RangedWeaponController: Assign PlayerInput.", this);
            return;
        }

        longGunAction =
            playerInput.actions.FindAction(longGunActionName, false);

        crossbowAction =
            playerInput.actions.FindAction(crossbowActionName, false);

        fireAction =
            playerInput.actions.FindAction(fireActionName, false);

        moveAction =
            playerInput.actions.FindAction(moveActionName, false);

        if (longGunAction != null)
            longGunAction.performed += OnLongGunPressed;
        else
            Debug.LogError(
                $"Input action not found: {longGunActionName}", this);

        if (crossbowAction != null)
            crossbowAction.performed += OnCrossbowPressed;
        else
            Debug.LogError(
                $"Input action not found: {crossbowActionName}", this);

        if (fireAction != null)
            fireAction.performed += OnFirePressed;
        else
            Debug.LogError(
                $"Input action not found: {fireActionName}", this);
    }

    private void UnbindInputActions()
    {
        if (longGunAction != null)
            longGunAction.performed -= OnLongGunPressed;

        if (crossbowAction != null)
            crossbowAction.performed -= OnCrossbowPressed;

        if (fireAction != null)
            fireAction.performed -= OnFirePressed;

        longGunAction = null;
        crossbowAction = null;
        fireAction = null;
        moveAction = null;
    }

    private void OnLongGunPressed(InputAction.CallbackContext context)
    {
        if (inventoryPanel != null && inventoryPanel.activeSelf)
            return;

        if (equippedWeapon == RangedWeapon.LongGun)
        {
            UnequipForWeaponSwitch();
            return;
        }

        EquipRangedWeapon(RangedWeapon.LongGun);
    }

    private void OnCrossbowPressed(InputAction.CallbackContext context)
    {
        if (inventoryPanel != null && inventoryPanel.activeSelf)
            return;

        if (equippedWeapon == RangedWeapon.Crossbow)
        {
            UnequipForWeaponSwitch();
            return;
        }

        EquipRangedWeapon(RangedWeapon.Crossbow);
    }

    private void EquipRangedWeapon(RangedWeapon weapon)
    {
        if (characterRenderer == null)
        {
            Debug.LogError(
                "RangedWeaponController: Character Renderer is missing.",
                this);
            return;
        }

        Sprite newSprite =
            weapon == RangedWeapon.LongGun
            ? longGunHeldSprite
            : crossbowHeldSprite;

        if (newSprite == null)
        {
            Debug.LogError(
                $"Assign the held sprite for {weapon}.", this);
            return;
        }

        if (swordController != null)
            swordController.UnequipForWeaponSwitch();

        if (!visualsOverridden)
        {
            originalSprite = characterRenderer.sprite;
            originalFlipX = characterRenderer.flipX;

            originalAnimatorEnabled =
                characterAnimator != null &&
                characterAnimator.enabled;

            if (originalAnimatorEnabled)
                characterAnimator.enabled = false;

            visualsOverridden = true;
        }

        equippedWeapon = weapon;
        characterRenderer.sprite = newSprite;
        characterRenderer.flipX = facingDirection.x < 0f;
    }

    public void UnequipForWeaponSwitch()
    {
        equippedWeapon = RangedWeapon.None;

        if (!visualsOverridden)
            return;

        if (characterAnimator != null && originalAnimatorEnabled)
        {
            characterAnimator.enabled = true;
        }
        else if (characterRenderer != null && originalSprite != null)
        {
            characterRenderer.sprite = originalSprite;
        }

        if (characterRenderer != null)
            characterRenderer.flipX = originalFlipX;

        visualsOverridden = false;
    }

    private void OnFirePressed(InputAction.CallbackContext context)
    {
        if (inventoryPanel != null && inventoryPanel.activeSelf)
            return;

        if (equippedWeapon == RangedWeapon.None)
            return;

        if (Time.time < nextFireTime)
            return;

        switch (equippedWeapon)
        {
            case RangedWeapon.LongGun:
                FireLongGun();
                break;

            case RangedWeapon.Crossbow:
                FireCrossbow();
                break;
        }
    }

    private void FireLongGun()
    {
        if (vitals == null || bulletPrefab == null)
            return;

        if (!vitals.TrySpendAmmo(1))
        {
            Debug.Log("Out of bullets.");
            return;
        }

        SpawnProjectile(
            bulletPrefab, bulletSpeed, bulletDamage);

        nextFireTime = Time.time + gunCooldown;

        if (audioSource != null && musketFireClip != null)
        {
            audioSource.PlayOneShot(musketFireClip);
        }
    }

    private void FireCrossbow()
    {
        if (vitals == null || arrowPrefab == null)
            return;

        if (!vitals.TrySpendArrows(1))
        {
            Debug.Log("Out of arrows.");
            return;
        }

        SpawnProjectile(
            arrowPrefab, arrowSpeed, arrowDamage);

        nextFireTime = Time.time + crossbowCooldown;

        if (audioSource != null && crossbowFireClip != null)
        {
            audioSource.PlayOneShot(crossbowFireClip);
        }
    }

    private void SpawnProjectile(
        Projectile prefab,
        float speed,
        int damage)
    {
        Vector2 direction = facingDirection.normalized;

        Vector3 spawnPosition =
            transform.position +
            (Vector3)(direction * 0.6f);

        Projectile projectile = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity);

        projectile.Launch(
            direction,
            speed,
            damage,
            gameObject);
    }
}
