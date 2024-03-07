using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Dart : ProjectileBase
{
    [SerializeField]
    private float _areaOfEffectRange = 0.25f;

    // properties true for all darts
    public static int Damage => (int)(BaseDamage * BaseDamagePercentage);
    public static float BaseDamagePercentage = 1f;
    public static float StartingCooldown = 1.8f;
    public static float Cooldown => BaseCooldownPercentage * StartingCooldown;

    public static float Speed = 10.0f;
    public static int AdditionalDarts = 0;
    public static float TimeElapsedSinceLastDart;
    public static bool IsShooting = false;

    private static float _distanceOutsidePlayer = 1.25f;
    private static float _shootingInterval = 0.4f; // time between consecutive darts

    private Vector3 _direction;

    public static IEnumerator ShootDarts(Dart prefab, GameObject player)
    {
        IsShooting = true;
        Vector3 direction = CalculateDirection(player);

        // shoot the base number of darts
        for (int i = 0; i < BaseCount; i++)
        {
            ShootADart(prefab, player, direction);
            if (AdditionalDarts > i)
            {
                ShootADart(prefab, player, direction * -1);
            }

            yield return new WaitForSeconds(_shootingInterval);
            // get new updates mouse coords inbetween shots
            direction = CalculateDirection(player);
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

        // initial position
        dart.transform.position =
            player.transform.position + direction.normalized * _distanceOutsidePlayer;

        dart.SetDirection(direction);
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

        _rigidbody2D.velocity = Vector3.Normalize(_direction) * Speed;
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy initialEnemy)
    {
        initialEnemy.TakeDamage(Damage);
        // why 5000? -- the result of experimenting with different values (!)
        initialEnemy.Knockback(_direction, 5000);

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
                    enemyHit.TakeDamage(Damage);
                }
            }
        }
    }

    public static void UpgradeDart(int level)
    {
        if (level == 1)
        {
            AdditionalDarts++;
        }
        else if (level == 2)
        {
            BaseDamagePercentage = 1.5f;
        }
        else if (level == 3)
        {
            BaseDamagePercentage = 2f;
            AdditionalDarts += 2;
        }
    }
}
