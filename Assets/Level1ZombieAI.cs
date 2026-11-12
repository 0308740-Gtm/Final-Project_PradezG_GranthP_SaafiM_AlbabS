using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1ZombieAI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerVitals playerVitals;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Animations")]
    [SerializeField] private string idleState = "Zombie_Idle";
    [SerializeField] private string walkState = "Zombie_Walk";
    [SerializeField] private string attackState1 = "Zombie_Attack_1";
    [SerializeField] private string attackState2 = "Zombie_Attack_2";

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private float attackRange = 1.2f;

    [Header("Attack")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1.5f;

    [SerializeField] private float attackWindup = 0.25f;
    [SerializeField] private float attackRecovery = 0.25f;

    private float nextAttackTime;

    private Coroutine attackRoutine;

    private string currentAnimation = "";

    private bool useSecondAttack;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }

        if (player != null && playerVitals == null)
        {
            playerVitals =
                player.GetComponent<PlayerVitals>();
        }

        if (player == null)
        {
            Debug.LogError(
                "Zombie cannot find Player. Check Player tag.");
            return;
        }

        PlayAnimation(idleState);
    }

    private void Update()
    {
        if (player == null || animator == null)
            return;

        Vector2 difference =
            (Vector2)player.position - (Vector2)transform.position;

        float distance = difference.magnitude;

        if (spriteRenderer != null &&
            Mathf.Abs(difference.x) > 0.05f)
        {
            spriteRenderer.flipX = difference.x < 0f;
        }

        if (attackRoutine != null)
            return;

        if (distance <= attackRange &&
            Time.time >= nextAttackTime)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
            return;
        }

        if (distance > attackRange)
            PlayAnimation(walkState);
        else
            PlayAnimation(idleState);
    }

    private void FixedUpdate()
    {
        if (player == null || rb == null)
            return;

        if (attackRoutine != null)
            return;

        Vector2 difference =
            (Vector2)player.position - rb.position;

        float distance = difference.magnitude;

        if (distance <= attackRange)
            return;

        Vector2 direction = difference.normalized;

        Vector2 nextPosition =
            rb.position +
            direction * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);
    }

    private IEnumerator AttackRoutine()
    {
        nextAttackTime = Time.time + attackCooldown;

        string attackState = useSecondAttack
            ? attackState2
            : attackState1;

        useSecondAttack = !useSecondAttack;

        PlayAnimation(attackState);

        yield return new WaitForSeconds(attackWindup);

        if (player != null &&
            playerVitals != null &&
            !playerVitals.IsDead)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position);

            if (distance <= attackRange)
                playerVitals.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackRecovery);

        attackRoutine = null;
    }

    private void PlayAnimation(string stateName)
    {
        if (animator == null ||
            currentAnimation == stateName)
            return;

        int stateHash = Animator.StringToHash(stateName);

        if (!animator.HasState(0, stateHash))
        {
            Debug.LogWarning(
                "Missing Animator state: " + stateName, this);
            return;
        }

        animator.Play(stateHash, 0, 0f);

        currentAnimation = stateName;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange);
    }
}
