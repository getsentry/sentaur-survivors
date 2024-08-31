using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;

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

    protected void SplashDamage(
        Vector2 origin,
        float radius,
        int damage,
        HashSet<Enemy> ignoreList = null
    )
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemyTarget = hit.collider.gameObject.GetComponent<Enemy>();
                if (ignoreList == null || !ignoreList.Contains(enemyTarget))
                {
                    enemyTarget.TakeDamage(damage);
                }
            }
        }
    }
}
