using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    public float GlobalDamageModifier = 1.0f;

    [SerializeField]
    public float GlobalCooldownModifier = 1.0f;

    [SerializeField]
    public int GlobalCountModifier = 1;

    private List<WeaponBase> _weapons = new List<WeaponBase>();

    private void Awake()
    {
        _weapons.AddRange(GetComponentsInChildren<WeaponBase>());
    }

    public void Add(WeaponBase weapon)
    {
        weapon.transform.parent = transform;

        _weapons.Add(weapon);
    }

    private void Update()
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
            GlobalDamageModifier = 1.3f;
        }
        else if (level == 2)
        {
            GlobalDamageModifier = 1.6f;
        }
        else if (level == 3)
        {
            GlobalDamageModifier = 2.0f;
        }
    }

    public void UpgradeCooldown(int level)
    {
        if (level == 1)
        {
            GlobalCooldownModifier = 0.8f;
        }
        else if (level == 2)
        {
            GlobalCooldownModifier = 0.6f;
        }
        else if (level == 3)
        {
            GlobalCooldownModifier = 0.3f;
        }
    }
}
