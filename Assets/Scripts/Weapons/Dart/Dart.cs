using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : WeaponBase
{
    // properties true for all darts
    public int Damage => (int)(BaseDamage * BaseDamagePercentage);
    public float BaseDamagePercentage = 1f;
    public float InitialCooldown = 1.8f;

    public float Speed = 10.0f;
    public int AdditionalDarts = 0;
    public float TimeElapsedSinceLastDart;
    public bool IsShooting = false;

    private float _distanceOutsidePlayer = 1.25f;
    private float _shootingInterval = 0.4f; // time between consecutive darts

    private GameObject _player;

    [SerializeField]
    private DartProjectile _dartProjectilePrefab;

    public void Start()
    {
        StartingCooldown = InitialCooldown;
        IsEnabled = true;
        _player = Player.Instance.gameObject;
    }

    public void Fire(Transform parent, Vector3 origin)
    {
        base.Fire();

        if (IsShooting)
        {
            // if the dart is already firing, exit early (we only start counting
            // after the dart has CEASED firing)
            return;
        }

        StartCoroutine(ShootDarts());
    }

    public IEnumerator ShootDarts()
    {
        IsShooting = true;
        Vector3 direction = CalculateDirection(_player);

        // shoot the base number of darts
        for (int i = 0; i < BaseCount; i++)
        {
            ShootADart(_dartProjectilePrefab, _player, direction);
            if (AdditionalDarts > i)
            {
                ShootADart(_dartProjectilePrefab, _player, direction * -1);
            }

            yield return new WaitForSeconds(_shootingInterval);
            // get new updates mouse coords inbetween shots
            direction = CalculateDirection(_player);
        }

        // accounting for case where # of backwards darts > # of forwards darts
        int remainingDarts = AdditionalDarts - BaseCount;
        if (remainingDarts > 0)
        {
            direction *= -1;
            for (int i = 0; i < remainingDarts; i++)
            {
                ShootADart(_dartProjectilePrefab, _player, direction);
                yield return new WaitForSeconds(_shootingInterval);
            }
        }

        TimeElapsedSinceLastDart = 0.0f;
        IsShooting = false;
        yield return null;
    }

    private void ShootADart(DartProjectile prefab, GameObject player, Vector3 direction)
    {
        DartProjectile dart = Instantiate(prefab);
        dart.Initialize(Damage, Speed);
        dart.transform.parent = player.transform.parent;

        // initial position
        dart.transform.position =
            player.transform.position + direction.normalized * _distanceOutsidePlayer;

        dart.SetDirection(direction);
    }

    private Vector3 CalculateDirection(GameObject player)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - player.transform.position;

        return direction;
    }

    public void Upgrade(int level)
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
