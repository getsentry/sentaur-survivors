using System.Collections;
using UnityEngine;

public class Schnitzel : WeaponBase
{
    // properties true for all schnitzel
    public int Damage => (int)(BaseDamage * BaseDamagePercentage);
    public float BaseDamagePercentage = 0.8f;
    public float InitialCooldown = 2.9f;

    public float Speed = 5.0f;
    public float TimeElapsedSinceLastSchnitzel;

    private float _distanceOutsidePlayer = 1.25f;
    private float _shootingInterval = 0.25f; // time between consecutive schnitzel

    public float Scale = 1.0f;

    [SerializeField]
    private SchnitzelProjectile _schnitzelPrefab;

    public void Start()
    {
        StartingCooldown = InitialCooldown;
    }

    public void Fire(Transform parentTransform, Vector2 originPosition)
    {
        base.Fire();

        StartCoroutine(ShootSchnitzels(_schnitzelPrefab, Player.Instance.gameObject));
    }

    public IEnumerator ShootSchnitzels(SchnitzelProjectile prefab, GameObject player)
    {
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
        schnitzel.Initialize(Damage, Speed);
        schnitzel.transform.parent = player.transform.parent;

        // initial position
        schnitzel.transform.position =
            player.transform.position + direction.normalized * _distanceOutsidePlayer;

        // adjust scale
        schnitzel.transform.localScale = new Vector3(Scale, Scale, Scale);

        schnitzel.SetDirection(direction);
    }

    public void Upgrade(int level)
    {
        if (level == 1)
        {
            IsEnabled = true;
        }
        else if (level == 2)
        {
            Scale *= 1.4f;
        }
        else if (level == 3)
        {
            Scale *= 1.3f;
        }
    }
}
