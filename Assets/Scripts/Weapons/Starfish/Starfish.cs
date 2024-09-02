using UnityEngine;

public class Starfish : WeaponBase
{
    [SerializeField]
    public float Duration = 5f;

    [SerializeField]
    private StarfishProjectile _starfishProjectilePrefab;

    public void Start()
    {
        // hack; when starfish inits, time elapsed starts at duration (see CanFire note)
        _timeElapsedSinceLastFire = Duration;
    }

    public override bool CanFire()
    {
        // starfish cooldown doesn't reset until duration is over
        return _isEnabled && _timeElapsedSinceLastFire >= Cooldown + Duration;
    }

    public override void Fire()
    {
        base.Fire();

        int numberOfStarfish = Count;
        float degreesBetweenStarfish = 360 / numberOfStarfish;

        for (int i = 0; i < Count; i++)
        {
            var starfish = Instantiate(_starfishProjectilePrefab);
            starfish.Initialize(Damage, Duration, degreesBetweenStarfish * i);
            starfish.transform.parent = this.transform;
        }
    }
}
