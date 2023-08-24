
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Dart : ProjectileBase
{
    // properties true for all darts
    public static int Damage = BaseDamage;
    public static float BaseDamagePercentage = 1f;
    public static float StartingCooldown = 1.8f;
    public static float Cooldown = 1.8f;
    public static float SpreadInDegrees = 10f;

    public static float Speed = 10.0f;
    public static int AdditionalDarts = 0;
    public static float TimeElapsedSinceLastDart;

    private Vector3 _direction;

    // Update is called once per frame
    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        // rotate the dart to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _rigidbody2D.velocity = _direction * Speed;
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }

    public static void UpgradeDart(int level) 
    {
        if (level == 1)
        {
            AdditionalDarts++;
        } 
        else if (level == 2)
        {
            BaseDamagePercentage = 1.2f;
            Damage = (int) (BaseDamage * BaseDamagePercentage);
        }
        else if (level == 3)
        {
            BaseDamagePercentage = 1.8f;
            Damage = (int) (BaseDamage * BaseDamagePercentage);
            AdditionalDarts += 2;
        }
    }
}
