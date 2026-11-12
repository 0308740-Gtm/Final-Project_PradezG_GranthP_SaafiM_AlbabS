using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
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
    [SerializeField] private int maxHealth = 100;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    [Header("Death")]
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private float destroyDelay = 1.5f;

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }

    public event System.Action<int, int> HealthChanged;

    public bool IsDead { get; private set; }

    private Collider2D[] zombieColliders;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        zombieColliders = GetComponentsInChildren<Collider2D>();
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0)
            return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);

        Debug.Log(
            $"Zombie Health: {CurrentHealth}/{MaxHealth}");

        HealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        Level1ZombieAI zombieAI =
            GetComponent<Level1ZombieAI>();

        if (zombieAI != null)
            zombieAI.enabled = false;

        if (animator != null)
            animator.enabled = false;

        if (spriteRenderer != null && deathSprite != null)
            spriteRenderer.sprite = deathSprite;

        foreach (Collider2D col in zombieColliders)
        {
            if (col != null)
                col.enabled = false;
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        Destroy(gameObject, destroyDelay);
    }

    private void TestDamage()
{
    TakeDamage(25);
}
}
