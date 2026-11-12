using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordZombieDamageBridge : MonoBehaviour
{
    [SerializeField] private SwordController swordController;

    [SerializeField] private int swordDamage = 40;

    private void Awake()
    {
        if (swordController == null)
            swordController = GetComponent<SwordController>();
    }

    private void OnEnable()
    {
        if (swordController != null)
            swordController.SwordHitDetected += OnSwordHit;
    }

    private void OnDisable()
    {
        if (swordController != null)
            swordController.SwordHitDetected -= OnSwordHit;
    }

    private void OnSwordHit(Collider2D hit)
    {
        if (hit == null)
            return;

        ZombieHealth zombie =
            hit.GetComponentInParent<ZombieHealth>();

        if (zombie != null)
            zombie.TakeDamage(swordDamage);

        BossHealth boss =
    hit.GetComponentInParent<BossHealth>();

        if (boss != null)
        {
            boss.TakeDamage(40);
        }
    }
}
