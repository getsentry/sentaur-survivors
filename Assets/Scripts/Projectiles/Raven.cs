using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raven : ProjectileBase
{

    // properties true for all ravens
    public static float Speed = 12.0f;
    public static int Damage = 10;
    public static float FireRate = 10f;
    public static GameObject FirstTarget = null;
    private static float _distanceOutsidePlayer = 2.0f;

    public int identifier;
    private Vector3 _direction;
    private GameObject _player;

    void Awake()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * Speed * Time.deltaTime;
    }

    public void TargetClosestEnemy() 
    {
        GameObject target = GetTarget();
        Vector3 direction = target.transform.position - _player.transform.position;
        SetDirection(direction);
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

        // rotate the raven to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    } 
}
