
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
    public static bool IsShooting = false;

    private static float _distanceOutsidePlayer = 2f;
    private static float _shootingInterval = 0.4f; // time between consecutive darts

    private Vector3 _direction;

    public static IEnumerator ShootDarts(Dart prefab, GameObject player)
    {
        IsShooting = true;
        Vector3 direction = CalculateDirection(player);

        for (int i = 0; i < BaseCount; i++)
        {
            ShootADart(prefab, player, direction);
            if (AdditionalDarts > i)
            {
                direction *= -1;
                ShootADart(prefab, player, direction);
                direction *= -1;
            }

            yield return new WaitForSeconds(_shootingInterval);            
        }

        // accounting for case where # of backwards darts > # of forwards darts
        int remainingDarts = AdditionalDarts - BaseCount;
        if (remainingDarts > 0) 
        {
            direction *= -1;
            for (int i = 0; i < remainingDarts; i++)
            {
                ShootADart(prefab, player, direction);
                yield return new WaitForSeconds(_shootingInterval);
            }
        }

        TimeElapsedSinceLastDart = 0.0f;
        IsShooting = false;
        yield return null;
    }

    private static void ShootADart(Dart prefab, GameObject player, Vector3 direction)
    {
        Dart dart = Instantiate(prefab);
        dart.transform.parent = player.transform.parent;
        dart.SetDirection(direction);
        dart.transform.position = player.transform.position + direction.normalized * _distanceOutsidePlayer;
    }

    private static Vector3 CalculateDirection(GameObject player)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - player.transform.position;
        
        return direction;
    }

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
