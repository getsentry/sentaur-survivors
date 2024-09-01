using System.Collections;
using UnityEngine;

public class Dart : WeaponBase
{
    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    private int _rearFiringDartCount = 0; // NOTE: rear-shooting darts

    [SerializeField]
    private float _spawnDistanceOutsidePlayer = 1.25f;

    [SerializeField]
    private float _shootingInterval = 0.4f; // time between consecutive darts

    [SerializeField]
    private float _areaOfEffectRadius = 0.25f;

    private bool _isShooting = false;
    private GameObject _player;

    [SerializeField]
    private DartProjectile _dartProjectilePrefab;

    public void Start()
    {
        _isEnabled = true;
        _player = Player.Instance.gameObject;
    }

    public override void Fire()
    {
        base.Fire();

        if (_isShooting)
        {
            // if the dart is already firing, exit early (we only start counting
            // after the dart has CEASED firing)
            return;
        }

        StartCoroutine(ShootDarts());
    }

    public IEnumerator ShootDarts()
    {
        _isShooting = true;
        Vector3 direction = CalculateDirection(_player);

        // shoot the base number of darts
        for (int i = 0; i < Count; i++)
        {
            ShootADart(_dartProjectilePrefab, _player, direction);
            if (_rearFiringDartCount > i)
            {
                ShootADart(_dartProjectilePrefab, _player, direction * -1);
            }

            yield return new WaitForSeconds(_shootingInterval);
            // get new updates mouse coords inbetween shots
            direction = CalculateDirection(_player);
        }

        // accounting for case where # of backwards darts > # of forwards darts
        int remainingDarts = _rearFiringDartCount - Count;
        if (remainingDarts > 0)
        {
            direction *= -1;
            for (int i = 0; i < remainingDarts; i++)
            {
                ShootADart(_dartProjectilePrefab, _player, direction);
                yield return new WaitForSeconds(_shootingInterval);
            }
        }

        _isShooting = false;
        yield return null;
    }

    private void ShootADart(DartProjectile prefab, GameObject player, Vector3 direction)
    {
        DartProjectile dart = Instantiate(prefab);
        dart.Initialize(Damage, _speed, _areaOfEffectRadius);
        dart.transform.parent = player.transform.parent;

        // initial position
        dart.transform.position =
            player.transform.position + direction.normalized * _spawnDistanceOutsidePlayer;

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
            _rearFiringDartCount++;
        }
        else if (level == 2)
        {
            _baseDamage *= 1.5f;
        }
        else if (level == 3)
        {
            _baseDamage *= 2f;
            _rearFiringDartCount += 2;
        }
    }
}
