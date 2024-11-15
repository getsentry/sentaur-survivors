using System.Collections.Generic;
using UnityEngine;

public class SchnitzelProjectile : ProjectileBase
{
    public static float MaxLifetimeInSeconds = 10.0f;
    public static float KnockbackForce = 1000f;

    private int _damage;
    private float _speed;

    private Vector3 _direction;
    private float _creationTime;

    private HashSet<int> enemiesHit = new HashSet<int>();

    public void Start()
    {
        _creationTime = Time.time;
    }

    public void Initialize(int damage, float speed)
    {
        _damage = damage;
        _speed = speed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction.y = Random.Range(1f, 1.6f);

        // schnitzel shoots either left or right (respects player crosshair), but
        // amount of force is random
        _direction.x = direction.x > 0 ? Random.Range(0.5f, 1f) : Random.Range(-0.5f, -1f);

        _direction.z = 0; // don't move in the z direction

        // rotate the dart to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _rigidbody2D.linearVelocity = _direction * _speed;
    }

    protected override void OnDamage(Enemy enemy)
    {
        // each projectile can only damage an enemy once
        if (enemiesHit.Contains(enemy.GetInstanceID()))
        {
            return;
        }

        enemiesHit.Add(enemy.GetInstanceID());
        base.OnDamage(enemy);
    }

    protected override void OnHit()
    {
        // no-op (don't destroy schnitzel on hit)
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy initialEnemy)
    {
        initialEnemy.TakeDamage(_damage);

        // why 1000? -- the result of experimenting with different values (!)
        initialEnemy.Knockback(_direction, KnockbackForce);
    }

    protected void Update()
    {
        // rotate the schnitzel 360 degrees over time
        transform.Rotate(0, 0, 360 * Time.deltaTime);

        // gravity pushes the schnitzel down
        _rigidbody2D.linearVelocity += Physics2D.gravity * Time.deltaTime;

        // if 10 seconds has passed destroy (off the screen somewhere)
        if (Time.time - _creationTime > MaxLifetimeInSeconds)
        {
            Destroy(gameObject);
        }
    }
}
