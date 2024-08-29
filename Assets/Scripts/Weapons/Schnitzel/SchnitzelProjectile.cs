using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;

public class SchnitzelProjectile : ProjectileBase
{
    [SerializeField]
    private float _areaOfEffectRange = 0.25f;

    private float _speed;
    private int _damage;

    // properties true for all schnitzel
    private static float _distanceOutsidePlayer = 1.25f;

    public static float MaxLifetimeInSeconds = 10.0f;
    public static float KnockbackForce = 1000f;

    public static float Scale = 1.0f;

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

    private static void ShootOneSchnitzel(
        SchnitzelProjectile prefab,
        GameObject player,
        Vector3 direction
    )
    {
        SchnitzelProjectile schnitzel = Instantiate(prefab);
        schnitzel.transform.parent = player.transform.parent;

        // initial position
        schnitzel.transform.position =
            player.transform.position + direction.normalized * _distanceOutsidePlayer;

        // adjust scale
        schnitzel.transform.localScale = new Vector3(Scale, Scale, Scale);

        schnitzel.SetDirection(direction);
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
        _rigidbody2D.velocity = _direction * _speed;
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy initialEnemy)
    {
        initialEnemy.TakeDamage(_damage);
        // why 1000? -- the result of experimenting with different values (!)
        initialEnemy.Knockback(_direction, KnockbackForce);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            initialEnemy.transform.position,
            _areaOfEffectRange,
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

    protected void Update()
    {
        // rotate the schnitzel 360 degrees over time
        transform.Rotate(0, 0, 360 * Time.deltaTime);

        // gravity pushes the schnitzel down
        _rigidbody2D.velocity += Physics2D.gravity * Time.deltaTime;

        // if 10 seconds has passed destroy (off the screen somewhere)
        if (Time.time - _creationTime > MaxLifetimeInSeconds)
        {
            Destroy(gameObject);
        }
    }
}
