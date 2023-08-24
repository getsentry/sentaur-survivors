using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raven : ProjectileBase
{

    // properties true for all ravens
    public static int Damage;
    public static float BaseDamagePercentage = 1.5f;
    public static float StartingCooldown = 6f;
    public static float Cooldown = 6f;
    public static bool IsEnabled = false;

    public static float Speed = 12.0f;
    public static int AdditionalRavens = 0;
    public static List<GameObject> CurrentTargets = new List<GameObject>{}; 
    public static float TimeElapsedSinceLastRaven;
    private static float _distanceOutsidePlayer = 2.0f;

    public int identifier;
    private Vector3 _direction;
    private GameObject _player;

    new void Awake()
    {
        base.Awake();

        _player = GameObject.Find("Player");
    }

    public void TargetClosestEnemy() 
    {
        GameObject target = GetTarget();
        if (target == null)
        {
            // nothing to target
            Destroy(gameObject);
            return;
        }
        Vector3 direction = target.transform.position - _player.transform.position;
        SetDirection(direction);

        // initial position
        transform.position = _player.transform.position + direction.normalized * _distanceOutsidePlayer;
    }

    private GameObject GetTarget() 
    {
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

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        _rigidbody2D.velocity = _direction * Speed;

        // rotate the raven to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }

    public static void UpgradeRaven(int level)
    {
        if (level == 1)
        {
            IsEnabled = true;
            Damage = (int) (BaseDamage * BaseDamagePercentage);
            Cooldown = StartingCooldown * BaseCooldownPercentage;
            TimeElapsedSinceLastRaven = Cooldown - 1f; // launch a raven as soon as it's enabled
        }
        else if (level == 2)
        {
            AdditionalRavens++;
        }
        else if (level == 3)
        {
            BaseDamagePercentage = 2f;
            Damage = (int) (BaseDamage * BaseDamagePercentage);
            StartingCooldown *= 0.8f;
            Cooldown = StartingCooldown * BaseCooldownPercentage;
        }
    }
}
