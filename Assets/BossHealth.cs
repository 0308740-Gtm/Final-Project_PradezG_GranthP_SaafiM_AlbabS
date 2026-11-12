using System;
using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField, Min(1)] private int maxHealth = 500;

    [Header("Death Settings")]
    [SerializeField] private float destroyDelay = 1.5f;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private BossAI bossAI;

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<int, int> HealthChanged;
    public event Action BossDefeated;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (bossAI == null)
            bossAI = GetComponent<BossAI>();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0)
            return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

        HealthChanged?.Invoke(CurrentHealth, maxHealth);

        Debug.Log($"{name} received {damage} damage. Health: {CurrentHealth}/{maxHealth}", this);

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        if (animator != null)
            animator.SetTrigger("Hurt");
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        Debug.Log($"{name} has been defeated!", this);

        if (bossAI != null)
            bossAI.StopBoss();

        if (animator != null)
            animator.SetTrigger("Die");

        BossDefeated?.Invoke();

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
            col.enabled = false;

        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}