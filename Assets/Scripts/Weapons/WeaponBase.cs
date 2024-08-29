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

    public void UpgradeDamage(int level)
    {
        if (level == 1)
        {
            BaseDamage = (int)(BaseDamage * 1.3);
        }
        else if (level == 2)
        {
            BaseDamage = (int)(BaseDamage * 1.6);
        }
        else if (level == 3)
        {
            BaseDamage = (int)(BaseDamage * 2);
        }
    }

    public void UpgradeCooldown(int level)
    {
        if (level == 1)
        {
            BaseCooldownPercentage = 0.8f;
        }
        else if (level == 2)
        {
            BaseCooldownPercentage = 0.6f;
        }
        else if (level == 3)
        {
            BaseCooldownPercentage = 0.3f;
        }
    }

    public void UpgradeCount(int level)
    {
        if (level == 3)
        {
            BaseCount += 2;
        }
        else
        {
            BaseCount++;
        }
    }
}
