using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 4f;

    private Rigidbody2D rb;
    private GameObject owner;
    private int damage;
    private bool launched;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(
        Vector2 direction,
        float speed,
        int projectileDamage,
        GameObject projectileOwner)
    {
        if (rb == null)
        {
            Debug.LogError(
                "RangedProjectile needs a Rigidbody2D.", this);

            Destroy(gameObject);
            return;
        }

        owner = projectileOwner;
        damage = projectileDamage;
        launched = true;

        direction = direction.normalized;

        rb.velocity = direction * speed;

        float angle = Mathf.Atan2(
            direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!launched)
            return;

        if (owner != null &&
            (other.transform == owner.transform ||
             other.transform.IsChildOf(owner.transform)))
        {
            return;
        }

        ZombieHealth zombie =
            other.GetComponentInParent<ZombieHealth>();

        if (zombie != null)
        {
            zombie.TakeDamage(damage);

            Debug.Log("Projectile damaged zombie!");
        }

        Destroy(gameObject);

        BossHealth boss =
    other.GetComponentInParent<BossHealth>();

        if (boss != null)
        {
            boss.TakeDamage(damage);
        }
    }
   }
