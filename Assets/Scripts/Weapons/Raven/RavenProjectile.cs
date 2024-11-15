using System.Collections.Generic;
using UnityEngine;

public class RavenProjectile : ProjectileBase
{
    [SerializeField]
    private Vector3 _direction;

    [SerializeField]
    private int _damage;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _areaOfEffectRadius;

    public void Initialize(int damage, float speed, float areaOfEffectRadius)
    {
        _damage = damage;
        _speed = speed;
        _areaOfEffectRadius = areaOfEffectRadius;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        _rigidbody2D.linearVelocity = Vector3.Normalize(_direction) * _speed;

        // rotate the raven to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Deal damage to the enemy because they were hit by the raven
    override protected void DamageEnemy(Enemy initialEnemy)
    {
        SplashDamage(initialEnemy.transform.position, _areaOfEffectRadius, _damage);
    }
}
