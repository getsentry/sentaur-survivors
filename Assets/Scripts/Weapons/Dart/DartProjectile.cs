using System.Collections.Generic;
using UnityEngine;

public class DartProjectile : ProjectileBase
{
    private Vector3 _direction;
    private int _damage;
    private float _speed;
    private float _areaOfEffectRadius;

    public void Initialize(int damage, float speed, float areaOfEffectModifier = 1.0f)
    {
        _damage = damage;
        _speed = speed;
        _areaOfEffectRadius = areaOfEffectModifier;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        // rotate the dart to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _rigidbody2D.velocity = Vector3.Normalize(_direction) * _speed;
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy initialEnemy)
    {
        // why 5000? -- the result of experimenting with different values (!)
        initialEnemy.Knockback(_direction, 5000);

        // deal damage to initial enemy
        initialEnemy.TakeDamage(_damage);

        // dart actually does some minor splash damage
        SplashDamage(
            initialEnemy.transform.position,
            _areaOfEffectRadius,
            _damage,
            new HashSet<Enemy>() { initialEnemy }
        );
    }
}
