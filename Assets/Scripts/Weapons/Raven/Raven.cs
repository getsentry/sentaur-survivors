using System.Collections.Generic;
using UnityEngine;

public class Raven : WeaponBase
{
    [SerializeField]
    [Tooltip("Projectile speed as it leaves player")]
    private float _speed = 12.0f;

    [SerializeField]
    [Tooltip("Modifies AOE of emitted projectiles")]
    private float _areaOfEffectRadius = 1.0f;

    [SerializeField]
    [Tooltip("Distance from player to spawn raven")]
    private float _spawnDistanceOutsidePlayer = 1.25f;

    [SerializeField]
    private RavenProjectile _ravenProjectilePrefab;

    private List<GameObject> _currentTargets = new List<GameObject> { };
    private GameObject _player;

    public void Start()
    {
        _player = Player.Instance.gameObject;
    }

    public override void Fire()
    {
        base.Fire();

        for (int i = 0; i < Count; i++)
        {
            var projectile = Instantiate(_ravenProjectilePrefab);

            projectile.Initialize(Damage, _speed, _areaOfEffectRadius);

            projectile.transform.position = _player.transform.position;
            projectile.transform.parent = _player.transform.parent;

            TargetClosestEnemy(projectile);
        }
        _currentTargets.Clear(); // reset raven targeting
    }

    public void TargetClosestEnemy(RavenProjectile projectile)
    {
        GameObject target = GetTarget();
        if (target == null)
        {
            // nothing to target
            Destroy(gameObject);
            return;
        }
        Vector3 direction = target.transform.position - _player.transform.position;
        projectile.SetDirection(direction);
        // initial position
        transform.position =
            _player.transform.position + direction.normalized * _spawnDistanceOutsidePlayer;
    }

    private GameObject GetTarget()
    {
        // TODO: this iteraates through every single enemy which is really bad
        // TODO: ^^^^^
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject target = null;
        float distance = Mathf.Infinity;
        Vector3 position = _player.transform.position;
        bool isAlreadyTargeted = false;
        foreach (GameObject enemy in enemies)
        {
            foreach (GameObject targetedEnemy in _currentTargets)
            {
                if (ReferenceEquals(enemy, targetedEnemy))
                {
                    // if this enemy is already targeted, skip over it to find our second closest so the second raven aims at a different enemy than the first raven
                    isAlreadyTargeted = true;
                    break;
                }
            }

            if (isAlreadyTargeted)
            {
                continue;
            }

            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                target = enemy;
                distance = curDistance;
            }
        }
        _currentTargets.Add(target);
        return target;
    }

    public void Upgrade(int level)
    {
        if (level == 1)
        {
            _isEnabled = true;
        }
        else if (level == 2)
        {
            _baseDamage = 2f;
            _startingCooldown *= 0.8f;
        }
        else if (level == 3)
        {
            _areaOfEffectRadius = 1.6f;
        }
    }
}
