using UnityEngine;

public class WeaponUpgradePath : UpgradePathBase
{
    [SerializeField]
    private WeaponBase _weaponPrefab;

    protected WeaponBase _weapon;

    public void Activate()
    {
        _weapon = Instantiate(_weaponPrefab);
        _weapon.transform.parent = FindObjectOfType<WeaponManager>().transform;

        _weapon.Enable();
    }
}
