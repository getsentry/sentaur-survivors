using UnityEngine;

public class DamageUpgradePath : UpgradePathBase
{
    [SerializeField]
    private float[] _damageModifiersPerLevel = { 1.3f, 1.25f, 1.25f };

    public override void UpgradeToLevel(int level)
    {
        var weaponManager = Player.Instance.WeaponManager;
        switch (_level)
        {
            case 1:
                weaponManager.GlobalDamageModifier *= _damageModifiersPerLevel[0];
                break;
            case 2:
                weaponManager.GlobalDamageModifier *= _damageModifiersPerLevel[1];
                break;
            case 3:
                weaponManager.GlobalDamageModifier *= _damageModifiersPerLevel[2];
                break;
        }
    }
}
