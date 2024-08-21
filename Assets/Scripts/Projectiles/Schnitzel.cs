using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Schnitzel : ProjectileBase
{
    [SerializeField]
    private float _areaOfEffectRange = 0.25f;

    // properties true for all schnitzel
    public static int Damage => (int)(BaseDamage * BaseDamagePercentage);
    public static float BaseDamagePercentage = 1f;
    public static float StartingCooldown = 1.8f;
    public static float Cooldown => BaseCooldownPercentage * StartingCooldown;

    public static float Speed = 5.0f;
    public static int AdditionalSchnitzels = 0;
    public static float TimeElapsedSinceLastSchnitzel;
    public static bool IsShooting = false;
    public static bool IsEnabled = false;

    private static float _distanceOutsidePlayer = 1.25f;
    private static float _shootingInterval = 0.4f; // time between consecutive schnitzel

    private Vector3 _direction;

    public static IEnumerator ShootSchnitzels(Schnitzel prefab, GameObject player)
    {
        IsShooting = true;
        Vector3 direction = CalculateDirection(player);

        // shoot the base number of schnitzel
        for (int i = 0; i < BaseCount; i++)
        {
            ShootOneSchnitzel(prefab, player, direction);

            yield return new WaitForSeconds(_shootingInterval);
            // get new updates mouse coords inbetween shots
            direction = CalculateDirection(player);
        }

        TimeElapsedSinceLastSchnitzel = 0.0f;
        IsShooting = false;
        yield return null;
    }

    protected override void OnHit()
    {
        // no-op (don't destroy schnitzel on hit)
    }

    private static void ShootOneSchnitzel(Schnitzel prefab, GameObject player, Vector3 direction)
    {
        Schnitzel schnitzel = Instantiate(prefab);
        schnitzel.transform.parent = player.transform.parent;

        // initial position
        schnitzel.transform.position =
            player.transform.position + direction.normalized * _distanceOutsidePlayer;

        schnitzel.SetDirection(direction);
    }

    private static Vector3 CalculateDirection(GameObject player)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - player.transform.position;

        return direction;
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
        _rigidbody2D.velocity = _direction * Speed;
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy initialEnemy)
    {
        initialEnemy.TakeDamage(Damage);
        // why 1000? -- the result of experimenting with different values (!)
        initialEnemy.Knockback(_direction, 1000);

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

    protected void Update()
    {
        // rotate the schnitzel 360 degrees over time
        transform.Rotate(0, 0, 360 * Time.deltaTime);

        // gravity pushes the schnitzel down
        _rigidbody2D.velocity += Physics2D.gravity * Time.deltaTime;
    }

    public static void UpgradeSchnitzel(int level)
    {
        if (level == 1)
        {
            IsEnabled = true;
            AdditionalSchnitzels++;
        }
        else if (level == 2)
        {
            BaseDamagePercentage = 1.5f;
        }
        else if (level == 3)
        {
            BaseDamagePercentage = 2f;
            AdditionalSchnitzels += 2;
        }
    }
}
