using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raven : WeaponBase
{
    // properties true for all ravens
    public int Damage => (int)(BaseDamage * BaseDamagePercentage);

    public float BaseDamagePercentage = 1.5f;
    public float StartingCooldown = 6f;
    public float Cooldown => BaseCooldownPercentage * StartingCooldown;
    public bool IsEnabled = true;

    public float Speed = 12.0f;
    public int AdditionalRavens = 0;
    public List<GameObject> CurrentTargets = new List<GameObject> { };
    public float TimeElapsedSinceLastRaven;
    private float _distanceOutsidePlayer = 1.25f;

    public float AreaOfEffectModifier = 1.0f;

    private GameObject _player;

    [SerializeField]
    private RavenProjectile _ravenPrefab;

    public void Awake()
    {
        _player = GameObject.Find("Player");
    }

    void Start()
    {
        BaseDamagePercentage = 1.5f;
        StartingCooldown = 6f;
        IsEnabled = false;

        Speed = 12.0f;
        AdditionalRavens = 0;
        CurrentTargets = new List<GameObject> { };
        _distanceOutsidePlayer = 1.25f;
    }

    // Update is called once per frame
    void Update() { }

    public void Fire(Transform parentTransform)
    {
        int numberOfRavens = RavenProjectile.BaseCount;
        for (int i = 0; i < numberOfRavens; i++)
        {
            var projectile = Instantiate(_ravenPrefab);

            projectile.Initialize(Damage, Speed, AreaOfEffectModifier);
            projectile.identifier = i;
            projectile.transform.parent = parentTransform;
            TargetClosestEnemy(projectile);
        }
        CurrentTargets.Clear(); // reset raven targeting
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
            _player.transform.position + direction.normalized * _distanceOutsidePlayer;
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
            foreach (GameObject targetedEnemy in CurrentTargets)
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
        CurrentTargets.Add(target);
        return target;
    }

    public void Upgrade(int level)
    {
        if (level == 1)
        {
            IsEnabled = true;
            TimeElapsedSinceLastRaven = Cooldown - 1f; // launch a raven as soon as it's enabled
        }
        else if (level == 2)
        {
            BaseDamagePercentage = 2f;
            StartingCooldown *= 0.8f;
        }
        else if (level == 3)
        {
            AreaOfEffectModifier = 1.6f;
        }
    }
}
