using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenProjectile : ProjectileBase
{
    [SerializeField]
    [Tooltip("How much AOE range this effect has on hit")]
    private float _areaOfEffectRange = 1.0f;

    public int identifier;

    private Vector3 _direction;
    private int _damage;
    private float _speed;
    private float _areaOfEffectModifier;

    public void Initialize(int damage, float speed, float areaOfEffectModifier)
    {
        _damage = damage;
        _speed = speed;
        _areaOfEffectModifier = areaOfEffectModifier;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        _rigidbody2D.velocity = _direction * _speed;

        // rotate the raven to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Deal damage to the enemy because they were hit by the raven
    override protected void DamageEnemy(Enemy initialEnemy)
    {
        // find all enemies within _areaOfEffectRange unit of enemy hit using raycast
        initialEnemy.TakeDamage(_damage);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            initialEnemy.transform.position,
            _areaOfEffectRange * _areaOfEffectModifier,
            Vector2.zero
        );
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemyHit = hit.collider.gameObject.GetComponent<Enemy>();
                // dont hit initial enemy twice
                if (enemyHit != initialEnemy)
                {
                    enemyHit.TakeDamage(_damage);
                }
            }
        }
    }
}