using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int BaseDamage = 10;

    public float BaseCooldownPercentage = 1f;

    public int BaseCount = 1;

    protected float TimeElapsedSinceLastFire = 0.0f;

    public bool IsEnabled = false;

    public float Cooldown => StartingCooldown * BaseCooldownPercentage;

    protected float StartingCooldown = 1.8f;

    private void Start()
    {
        BaseDamage = 10;
        BaseCooldownPercentage = 1f;
        BaseCount = 1;
    }

    private void Update()
    {
        TimeElapsedSinceLastFire += Time.deltaTime;
    }

    public bool CanFire()
    {
        return IsEnabled && TimeElapsedSinceLastFire >= Cooldown;
    }

    protected virtual void Fire()
    {
        TimeElapsedSinceLastFire = 0.0f;
    }
}
