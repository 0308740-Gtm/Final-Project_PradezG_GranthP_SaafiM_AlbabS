using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Character")]
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Animator characterAnimator;

    [Header("Sword Sprites")]
    [SerializeField] private Sprite swordIdleSprite;
    [SerializeField] private Sprite swordAttackFrame1;
    [SerializeField] private Sprite swordAttackFrame2;

    [Header("Animation Settings")]
    [SerializeField, Min(0.01f)]
    private float frameDuration = 0.10f;

    [SerializeField, Min(0.05f)]
    private float attackCooldown = 0.35f;

    [Header("Input Action Names")]
    [SerializeField] private string equipActionName = "SelectSlot4";
    [SerializeField] private string fireActionName = "Fire";
    [SerializeField] private string moveActionName = "Move";

    [Header("Optional Hit Detection")]
    [SerializeField] private LayerMask hittableLayers = ~0;

    [SerializeField, Min(0.1f)]
    private float hitDistance = 0.65f;

    [SerializeField, Min(0.1f)]
    private float hitRadius = 0.45f;

    public event Action<Collider2D> SwordHitDetected;

    private InputAction equipAction;
    private InputAction fireAction;
    private InputAction moveAction;

    private bool swordEquipped;
    private bool animatorWasEnabled;
    private Sprite previousSprite;

    private float nextAttackTime;
    private Vector2 facingDirection = Vector2.right;
    private Coroutine attackRoutine;

    [SerializeField] private RangedWeaponController rangedWeaponController;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip swordSwingClip;

    public void PerformSwordAttack()
    {
        if (audioSource != null && swordSwingClip != null)
        {
            audioSource.PlayOneShot(swordSwingClip);
        }
    }

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (inventory == null)
            inventory = GetComponent<PlayerInventory>();

        if (characterRenderer == null)
            characterRenderer = GetComponentInChildren<SpriteRenderer>();

        if (characterAnimator == null)
            characterAnimator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (playerInput == null || playerInput.actions == null)
        {
            Debug.LogError(
                "SwordController: Assign the PlayerInput component.", this);
            return;
        }

        equipAction =
            playerInput.actions.FindAction(equipActionName, false);

        fireAction =
            playerInput.actions.FindAction(fireActionName, false);

        moveAction =
            playerInput.actions.FindAction(moveActionName, false);

        if (equipAction != null)
            equipAction.performed += OnEquipPressed;
        else
            Debug.LogError($"Input action not found: {equipActionName}", this);

        if (fireAction != null)
            fireAction.performed += OnFirePressed;
        else
            Debug.LogError($"Input action not found: {fireActionName}", this);

        if (moveAction == null)
            Debug.LogWarning(
                $"Input action not found: {moveActionName}. " +
                "The sword will face right by default.", this);
    }

    private void OnDisable()
    {
        if (equipAction != null)
            equipAction.performed -= OnEquipPressed;

        if (fireAction != null)
            fireAction.performed -= OnFirePressed;

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        if (swordEquipped)
            UnequipSword();

        equipAction = null;
        fireAction = null;
        moveAction = null;
    }

    private void Update()
    {
        if (moveAction != null && moveAction.enabled)
        {
            Vector2 movement = moveAction.ReadValue<Vector2>();

            if (movement.sqrMagnitude > 0.01f)
                facingDirection = movement.normalized;
        }

        if (swordEquipped && characterRenderer != null)
            characterRenderer.flipX = facingDirection.x < 0f;
    }

    private void OnEquipPressed(InputAction.CallbackContext context)
    {
        if (inventoryPanel != null && inventoryPanel.activeSelf)
        {
            return;
        }

        if (swordEquipped)
        {
            UnequipSword();
        }
        else
        {
            EquipSword();
        }

        if (rangedWeaponController != null)
            rangedWeaponController.UnequipForWeaponSwitch();
    }

    private void EquipSword()
    {
        if (characterRenderer == null || swordIdleSprite == null)
        {
            Debug.LogError(
                "Assign Character Renderer and Sword Idle Sprite.", this);
            return;
        }

        previousSprite = characterRenderer.sprite;

        animatorWasEnabled =
            characterAnimator != null && characterAnimator.enabled;

        if (animatorWasEnabled)
            characterAnimator.enabled = false;

        swordEquipped = true;
        characterRenderer.sprite = swordIdleSprite;
        characterRenderer.flipX = facingDirection.x < 0f;
    }

    private void UnequipSword()
    {
        swordEquipped = false;

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        if (characterAnimator != null && animatorWasEnabled)
        {
            characterAnimator.enabled = true;
        }
        else if (characterRenderer != null && previousSprite != null)
        {
            characterRenderer.sprite = previousSprite;
        }

        animatorWasEnabled = false;
    }

    private void OnFirePressed(InputAction.CallbackContext context)
    {
        if (!swordEquipped)
            return;

        if (inventoryPanel != null && inventoryPanel.activeSelf)
            return;

        if (Time.time < nextAttackTime || attackRoutine != null)
            return;

        nextAttackTime = Time.time + attackCooldown;
        attackRoutine = StartCoroutine(PlaySwordAttack());
    }

    private IEnumerator PlaySwordAttack()
    {
        if (characterRenderer == null)
        {
            attackRoutine = null;
            yield break;
        }

        PerformSwordAttack();

        if (swordAttackFrame1 != null)
            characterRenderer.sprite = swordAttackFrame1;

        yield return new WaitForSeconds(frameDuration);

        if (!swordEquipped)
        {
            attackRoutine = null;
            yield break;
        }

        if (swordAttackFrame2 != null)
            characterRenderer.sprite = swordAttackFrame2;

        DetectHits();

        yield return new WaitForSeconds(frameDuration);

        if (swordEquipped && swordIdleSprite != null)
            characterRenderer.sprite = swordIdleSprite;

        attackRoutine = null;
    }

    private void DetectHits()
    {
        Vector2 centre =
            (Vector2)transform.position +
            facingDirection * hitDistance;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            centre, hitRadius, hittableLayers);

        HashSet<Transform> alreadyReported = new HashSet<Transform>();

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
                continue;

            Transform target = hit.transform.root;

            if (!alreadyReported.Add(target))
                continue;

            SwordHitDetected?.Invoke(hit);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 direction = facingDirection.sqrMagnitude > 0f
            ? facingDirection.normalized
            : Vector2.right;

        Vector2 centre =
            (Vector2)transform.position + direction * hitDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centre, hitRadius);
    }

    public void UnequipForWeaponSwitch()
    {
        if (swordEquipped)
            UnequipSword();
    }
}