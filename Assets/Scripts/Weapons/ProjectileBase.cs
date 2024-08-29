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

    public static void Reset()
    {
        BaseDamage = 10;
        BaseCooldownPercentage = 1f;
    }

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

            SoundManager.Instance.PlayHitSound();

            OnDamage(enemy);
            OnHit();
        }
        else if (other.gameObject.tag == "Barrier")
        {
            OnHit();
        }
    }

    protected virtual void OnDamage(Enemy enemy)
    {
        DamageEnemy(enemy);
    }

    protected virtual void OnHit()
    {
        Destroy(this.gameObject);
    }

    // Deal damage to the enemy because they were hit by a projectile
    virtual protected void DamageEnemy(Enemy enemy) { }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
