using UnityEngine;

class CooldownUpgradePath : UpgradePathBase
{
    [SerializeField]
    private float[] _cooldownModifiersPerLevel = { 0.8f, 0.75f, 0.5f };

    public override void UpgradeToLevel(int level)
    {
        var weaponManager = FindObjectOfType<WeaponManager>();
        switch (level)
        {
            case 1:
                weaponManager.GlobalCooldownModifier *= _cooldownModifiersPerLevel[0];
                break;
            case 2:
                weaponManager.GlobalCooldownModifier *= _cooldownModifiersPerLevel[1];
                break;
            case 3:
                weaponManager.GlobalCooldownModifier *= _cooldownModifiersPerLevel[2];
                break;
        }
    }
}
