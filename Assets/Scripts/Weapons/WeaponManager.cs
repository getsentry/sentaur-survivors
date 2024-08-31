using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    public int BaseDamage = 10;

    [SerializeField]
    public float BaseCooldownPercentage = 1f;

    [SerializeField]
    public int BaseCount = 1;

    private List<WeaponBase> _weapons = new List<WeaponBase>();

    public void Awake()
    {
        _weapons.AddRange(GetComponentsInChildren<WeaponBase>());
    }

    public void Update()
    {
        foreach (var weapon in _weapons)
        {
            if (weapon.CanFire())
            {
                weapon.Fire();
            }
        }
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
