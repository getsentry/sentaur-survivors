using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raven : ProjectileBase
{

    // properties true for all ravens
    public static int Damage;
    public static float BaseDamagePercentage = 0.5f;
    public static float Cooldown;
    public static float BaseCooldownPercentage = 3f;

    public static float Speed = 12.0f;
    public static int AdditionalRavens = 0;
    public static GameObject FirstTarget = null;
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
        foreach (GameObject enemy in enemies)
        {
            if (ReferenceEquals(enemy, FirstTarget))
            {
                // if this enemy is already targeted, skip over it to find our second closest so the second raven aims at a different enemy than the first raven
                continue;
            }

            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                if (identifier == 0)
                {
                    // only set this if this Raven is the first one 
                    FirstTarget = enemy;
                }
                target = enemy;
                distance = curDistance;
            }
        }
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
        if (level == 2)
        {
            AdditionalRavens++;
        }
        else if (level == 3)
        {
            BaseDamagePercentage = 0.75f;
            Damage = (int) (BaseDamage * BaseDamagePercentage);
            BaseCooldownPercentage = 2f;
            Cooldown = BaseCooldown * BaseCooldownPercentage;
        }
    }
}
