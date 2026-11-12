
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 25;

    [Header("Collision Detection")]
    [SerializeField] private LayerMask enemyLayers = ~0;
    [SerializeField] private float hitRadius = 0.15f;

    private Vector2 previousPosition;
    private bool hasHit;

    private void OnEnable()
    {
        previousPosition = transform.position;
        hasHit = false;
    }

    private void LateUpdate()
    {
        if (hasHit)
            return;

        Vector2 currentPosition = transform.position;
        Vector2 movement = currentPosition - previousPosition;

        if (movement.sqrMagnitude > 0.000001f)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                previousPosition,
                hitRadius,
                movement.normalized,
                movement.magnitude,
                enemyLayers
            );

            
            RaycastHit2D closestHit = default;
            float closestDistance = float.MaxValue;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (!HasEnemyHealth(hit.collider))
                    continue;

                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestHit = hit;
                }
            }

            if (closestHit.collider != null)
            {
                ApplyDamage(closestHit.collider);
            }
        }

        if (!hasHit)
        {
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(
                currentPosition,
                hitRadius,
                enemyLayers
            );

            foreach (Collider2D enemy in overlaps)
            {
                if (enemy != null && HasEnemyHealth(enemy))
                {
                    ApplyDamage(enemy);
                    break;
                }
            }
        }

        previousPosition = currentPosition;
    }

    private bool HasEnemyHealth(Collider2D hit)
    {
        return FindInParents<ZombieHealth>(hit.transform) != null ||
               FindInParents<BossHealth>(hit.transform) != null;
    }

    private void ApplyDamage(Collider2D hit)
    {
        if (hasHit || hit == null)
            return;

        ZombieHealth zombie =
            FindInParents<ZombieHealth>(hit.transform);

        BossHealth boss =
            FindInParents<BossHealth>(hit.transform);

        if (zombie == null && boss == null)
            return;

        hasHit = true;

        if (zombie != null)
        {
            zombie.TakeDamage(damage);

            Debug.Log(
                $"PROJECTILE HIT ZOMBIE: {zombie.name}, Damage: {damage}"
            );
        }
        else if (boss != null)
        {
            boss.TakeDamage(damage);

            Debug.Log(
                $"PROJECTILE HIT BOSS: {boss.name}, Damage: {damage}"
            );
        }

        Destroy(gameObject);
    }

    private T FindInParents<T>(Transform target)
        where T : Component
    {
        while (target != null)
        {
            T component = target.GetComponent<T>();

            if (component != null)
                return component;

            target = target.parent;
        }

        return null;
    }

    public void SetDamage(int amount)
    {
        damage = Mathf.Max(1, amount);
    }
}
