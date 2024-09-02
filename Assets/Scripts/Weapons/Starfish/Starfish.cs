using UnityEngine;

public class Starfish : WeaponBase
{
    [SerializeField]
    private float _duration = 5f;

    [SerializeField]
    private StarfishProjectile _starfishProjectilePrefab;

    public void Start()
    {
        // hack; when starfish inits, time elapsed starts at duration (see CanFire note)
        _timeElapsedSinceLastFire = _duration;
    }

    public override bool CanFire()
    {
        // starfish cooldown doesn't reset until duration is over
        return _isEnabled && _timeElapsedSinceLastFire >= Cooldown + _duration;
    }

    public override void Fire()
    {
        base.Fire();

        int numberOfStarfish = Count;
        float degreesBetweenStarfish = 360 / numberOfStarfish;

        for (int i = 0; i < Count; i++)
        {
            var starfish = Instantiate(_starfishProjectilePrefab);
            starfish.Initialize(Damage, _duration, degreesBetweenStarfish * i);
            starfish.transform.parent = this.transform;
        }
    }

    public void Upgrade(int level)
    {
        if (level == 1)
        {
            _isEnabled = true;
        }
        else if (level == 2)
        {
            _duration *= 1.2f;
            BaseDamage *= 1.2f;
        }
        else if (level == 3)
        {
            _duration *= 1.5f;
            BaseCooldown *= 0.7f;
        }
    }
}
