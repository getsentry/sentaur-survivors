using System.Collections;
using UnityEngine;

public class Schnitzel : WeaponBase
{
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private float _spawnDistanceOutsidePlayer = 1.25f;

    [SerializeField]
    private float _shootingInterval = 0.25f; // time between consecutive schnitzel

    [SerializeField]
    public float Scale = 1.0f;

    [SerializeField]
    private SchnitzelProjectile _schnitzelProjectilePrefab;

    public override void Fire()
    {
        base.Fire();

        var player = Player.Instance.gameObject;
        StartCoroutine(ShootSchnitzels(player));
    }

    public IEnumerator ShootSchnitzels(GameObject player)
    {
        SchnitzelProjectile schnitzelProjectilePrefab = _schnitzelProjectilePrefab;
        Vector3 direction = CalculateDirection(player);

        // shoot the base number of schnitzel
        for (int i = 0; i < Count; i++)
        {
            ShootOneSchnitzel(schnitzelProjectilePrefab, player, direction);

            yield return new WaitForSeconds(_shootingInterval);
            // get new updates mouse coords inbetween shots
            direction = CalculateDirection(player);
        }

        yield return null;
    }

    private Vector3 CalculateDirection(GameObject player)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - player.transform.position;

        return direction;
    }

    private void ShootOneSchnitzel(SchnitzelProjectile prefab, GameObject player, Vector3 direction)
    {
        SchnitzelProjectile schnitzel = Instantiate(prefab);
        schnitzel.Initialize(Damage, _speed);
        schnitzel.transform.parent = player.transform.parent;

        // initial position
        schnitzel.transform.position =
            player.transform.position + direction.normalized * _spawnDistanceOutsidePlayer;

        // adjust scale
        schnitzel.transform.localScale = new Vector3(Scale, Scale, Scale);

        schnitzel.SetDirection(direction);
    }
}
