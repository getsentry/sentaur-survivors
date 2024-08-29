using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected float TimeElapsedSinceLastFire = 0.0f;

    public bool IsEnabled = false;

    protected WeaponManager _weaponManager;

    protected float StartingCooldown = 1.8f;
    public float Cooldown => StartingCooldown * BaseCooldownPercentage;

    // proxied
    public int BaseDamage => _weaponManager.BaseDamage;
    public float BaseCooldownPercentage => _weaponManager.BaseCooldownPercentage;
    public int BaseCount => _weaponManager.BaseCount;

    private void Awake()
    {
        _weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
    }

    protected virtual void Update()
    {
        TimeElapsedSinceLastFire += Time.deltaTime;
    }

    public virtual bool CanFire()
    {
        return IsEnabled && TimeElapsedSinceLastFire >= Cooldown;
    }

    protected virtual void Fire()
    {
        TimeElapsedSinceLastFire = 0.0f;
    }
}
