using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    public static int BaseDamage = 10;
    public static float BaseCooldownPercentage = 1f;
    public static int BaseCount = 1;

    protected void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Hitbox")
        {
            var hitbox = other.gameObject.GetComponent<Hitbox>();
            var enemy = hitbox.Enemy;

            DamageEnemy(enemy);

            SoundManager.Instance.PlayHitSound();

            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Barrier")
        {
            // Destroy the projectile
            Destroy(this.gameObject);
        }
    }

    // Deal damage to the enemy because they were hit by a projectile
    virtual protected void DamageEnemy(Enemy enemy) { }

    public static void UpgradeDamage(int level)
    {
        if (level == 1)
        {
            BaseDamage = (int)(BaseDamage * 1.3);
        }
        else if (level == 2)
        {
            BaseDamage = (int)(BaseDamage * 1.6);
        }
        else if (level == 3)
        {
            BaseDamage = (int)(BaseDamage * 2);
        }
    }

    public static void UpgradeCooldown(int level)
    {
        if (level == 1)
        {
            BaseCooldownPercentage = 0.8f;
        }
        else if (level == 2)
        {
            BaseCooldownPercentage = 0.6f;
        }
        else if (level == 3)
        {
            BaseCooldownPercentage = 0.3f;
        }
    }

    public static void UpgradeCount(int level)
    {
        if (level == 3)
        {
            BaseCount += 2;
        }
        else
        {
            BaseCount++;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
