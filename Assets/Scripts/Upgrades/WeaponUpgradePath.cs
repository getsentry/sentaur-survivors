using UnityEngine;

public abstract class WeaponUpgradePath : UpgradePathBase
{
    [SerializeField]
    private WeaponBase _weaponPrefab;

    protected WeaponBase _weapon;

    public void Activate()
    {
        _weapon = Instantiate(_weaponPrefab);
        _weapon.Enable();

        Player.Instance.WeaponManager.Add(_weapon);
    }
}
