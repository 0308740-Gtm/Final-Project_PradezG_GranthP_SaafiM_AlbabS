using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackDistance = 1.8f;

    [Header("Attack")]
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackCooldown = 2.5f;
    [SerializeField] private float attackDuration = 0.8f;
    [SerializeField] private float impactDelay = 0.35f;
    [SerializeField] private float attackRange = 2f;

    [Header("Boss Phases")]
    [SerializeField] private bool usePhases = true;
    [SerializeField] private float phaseTwoHealth = 0.5f;
    [SerializeField] private float phaseThreeHealth = 0.25f;
    [SerializeField] private float phaseTwoSpeedMultiplier = 1.25f;
    [SerializeField] private float phaseThreeSpeedMultiplier = 1.5f;
    [SerializeField] private float phaseTwoCooldownMultiplier = 0.8f;
    [SerializeField] private float phaseThreeCooldownMultiplier = 0.65f;

    private bool activated = true; // Set to true by default for auto-start
    private bool isAttacking;
    private bool isStopped;

    private float nextAttackTime;
    private float currentSpeed;
    private float currentCooldown;

    private int currentPhase = 1;

    public bool IsDefeated => bossHealth != null && bossHealth.IsDead;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (bossHealth == null) bossHealth = GetComponent<BossHealth>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        currentSpeed = moveSpeed;
        currentCooldown = attackCooldown;
    }

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (!activated || isStopped || IsDefeated)
            return;

        if (player == null)
            FindPlayer();

        if (player == null)
            return;

        UpdatePhase();

        if (isAttacking)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackDistance)
        {
            SetMoving(false);

            if (Time.time >= nextAttackTime)
                StartCoroutine(AttackRoutine());
        }
        else
        {
            SetMoving(true);
            FacePlayer();
        }
    }

    private void FixedUpdate()
    {
        if (!activated || isStopped || IsDefeated || player == null || isAttacking)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
        }
    }

    public void ActivateBoss()
    {
        if (IsDefeated)
            return;

        activated = true;
        FindPlayer();
    }

    public void StopBoss()
    {
        isStopped = true;
        activated = false;
        isAttacking = false;

        StopAllCoroutines();
        SetMoving(false);
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
    }

    private void FacePlayer()
    {
        if (player == null || spriteRenderer == null)
            return;

        spriteRenderer.flipX = player.position.x < transform.position.x;
    }

    private void SetMoving(bool moving)
    {
        if (animator != null)
            animator.SetBool("IsMoving", moving);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        nextAttackTime = Time.time + currentCooldown;

        SetMoving(false);

        if (animator != null)
            animator.SetTrigger("Attack");

        yield return new WaitForSeconds(impactDelay);

        TryDamagePlayer();

        float remaining = Mathf.Max(0f, attackDuration - impactDelay);
        yield return new WaitForSeconds(remaining);

        isAttacking = false;
    }

    private void TryDamagePlayer()
    {
        if (player == null || IsDefeated)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
            return;

        PlayerVitals vitals = player.GetComponent<PlayerVitals>();
        if (vitals == null) vitals = player.GetComponentInParent<PlayerVitals>();
        if (vitals == null) vitals = player.GetComponentInChildren<PlayerVitals>(true);

        if (vitals == null)
        {
            Debug.LogError("BossAI cannot find PlayerVitals on the Player!", player);
            return;
        }

        if (vitals.IsDead)
            return;

        vitals.TakeDamage(attackDamage);
    }

    private void UpdatePhase()
    {
        if (!usePhases || bossHealth == null)
            return;

        float healthPercentage = (float)bossHealth.CurrentHealth / bossHealth.MaxHealth;

        if (healthPercentage <= phaseThreeHealth && currentPhase < 3)
        {
            currentPhase = 3;
            currentSpeed = moveSpeed * phaseThreeSpeedMultiplier;
            currentCooldown = attackCooldown * phaseThreeCooldownMultiplier;
            attackDamage = 45;
        }
        else if (healthPercentage <= phaseTwoHealth && currentPhase < 2)
        {
            currentPhase = 2;
            currentSpeed = moveSpeed * phaseTwoSpeedMultiplier;
            currentCooldown = attackCooldown * phaseTwoCooldownMultiplier;
            attackDamage = 30;
        }
    }
}