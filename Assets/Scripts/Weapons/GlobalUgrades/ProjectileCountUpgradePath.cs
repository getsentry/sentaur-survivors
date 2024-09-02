using UnityEngine;

class ProjectileCountUpgradePath : UpgradePathBase
{
    [SerializeField]
    private int[] _countModifiersPerLevel = { 2, 3, 5 };

    public override void UpgradeToLevel(int level)
    {
        // get WeaponManager
        var weaponManager = FindObjectOfType<WeaponManager>();
        switch (level)
        {
            case 1:
                weaponManager.GlobalCountModifier = _countModifiersPerLevel[0];
                break;
            case 2:
                weaponManager.GlobalCountModifier = _countModifiersPerLevel[1];
                break;
            case 3:
                weaponManager.GlobalCountModifier = _countModifiersPerLevel[2];
                break;
        }
    }
}
