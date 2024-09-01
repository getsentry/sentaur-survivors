using System;
using System.Collections.Generic;
using UnityEngine;

public class Raven : WeaponBase
{
    [SerializeField]
    [Tooltip("Projectile speed as it leaves player")]
    private float _speed = 12.0f;

    [SerializeField]
    [Tooltip("Modifies AOE of emitted projectiles")]
    public float AreaOfEffectRadius = 1.0f;

    [SerializeField]
    [Tooltip("Distance from player to spawn raven")]
    private float _spawnDistanceOutsidePlayer = 1.25f;

    [SerializeField]
    [Tooltip("Distance from player to detect enemies")]
    private float _detectRadius = 12.0f;

    [SerializeField]
    private RavenProjectile _ravenProjectilePrefab;

    private GameObject _player;

    public void Start()
    {
        _player = Player.Instance.gameObject;
    }

    public override void Fire()
    {
        base.Fire();

        var targets = GetTargets(_detectRadius, Count);

        // foreach
        foreach (var target in targets)
        {
            var projectile = Instantiate(_ravenProjectilePrefab);

            projectile.Initialize(Damage, _speed, AreaOfEffectRadius);
            projectile.transform.parent = _player.transform.parent;

            Vector3 direction = target.transform.position - _player.transform.position;
            projectile.SetDirection(direction);

            // initial position
            projectile.transform.position =
                _player.transform.position + direction.normalized * _spawnDistanceOutsidePlayer;
        }
    }

    private List<GameObject> GetEnemiesWithinRange(float range)
    {
        List<GameObject> enemies = new List<GameObject> { };

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            _player.transform.position,
            range,
            Vector2.zero
        );
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(hit.collider.gameObject);
            }
        }
        return enemies;
    }

    private List<GameObject> GetTargets(float radius, int count)
    {
        List<GameObject> enemies = GetEnemiesWithinRange(radius);
        List<GameObject> targets = new List<GameObject> { };

        Vector3 playerPosition = _player.transform.position;

        var maxTargets = Math.Min(count, enemies.Count);
        for (int i = 0; i < maxTargets; i++)
        {
            // find the next closest
            float shortestDistance = Mathf.Infinity;
            GameObject target = null;

            foreach (GameObject enemy in enemies)
            {
                Vector3 diff = enemy.transform.position - playerPosition;
                float enemyDistance = diff.sqrMagnitude;
                if (enemyDistance < shortestDistance)
                {
                    target = enemy;
                    shortestDistance = enemyDistance;
                }
            }

            enemies.Remove(target);
            targets.Add(target);
        }
        return targets;
    }

    public void Upgrade(int level)
    {
        if (level == 1)
        {
            _isEnabled = true;
        }
        else if (level == 2)
        {
            BaseDamage = 2f;
            _baseCooldown *= 0.8f;
        }
        else if (level == 3)
        {
            AreaOfEffectRadius = 1.6f;
        }
    }
}
