using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfish : WeaponBase
{
    // properties true for all starfish
    public int Damage => (int)(BaseDamage * BaseDamagePercentage);
    public float BaseDamagePercentage = 0.85f;
    public float InitialCooldown = 1f;

    public float Duration = 5f;

    public float DegreesToNextStarfish;
    public int identifier;

    [SerializeField]
    private StarfishProjectile _starfishProjectilePrefab;

    public void Start()
    {
        StartingCooldown = InitialCooldown;

        // hack; when starfish inits, time elapsed starts at duration (see CanFire note)
        TimeElapsedSinceLastFire = Duration;
    }

    public override bool CanFire()
    {
        // starfish cooldown doesn't reset until duration is over
        return IsEnabled && TimeElapsedSinceLastFire >= Cooldown + Duration;
    }

    public void Fire(Transform parentTransform, Vector3 origin)
    {
        base.Fire();

        int numberOfStarfish = BaseCount;
        float degreesBetweenStarfish = 360 / numberOfStarfish;

        for (int i = 0; i < numberOfStarfish; i++)
        {
            _starfishProjectilePrefab.DegreesToNextStarfish = degreesBetweenStarfish;
            _starfishProjectilePrefab.identifier = i;
            var starfish = Instantiate(_starfishProjectilePrefab);
            starfish.Initialize(Damage, Duration, DegreesToNextStarfish);
            starfish.transform.parent = parentTransform;
        }
    }

    public void Upgrade(int level)
    {
        if (level == 1)
        {
            IsEnabled = true;
        }
        else if (level == 2)
        {
            Duration *= 1.2f;
            BaseDamagePercentage = 1.2f;
        }
        else if (level == 3)
        {
            Duration *= 1.5f;
            StartingCooldown *= 0.7f;
        }
    }
}
