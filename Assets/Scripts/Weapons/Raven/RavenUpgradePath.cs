using UnityEngine;

public class RavenUpgradePath : WeaponUpgradePath
{
    [SerializeField]
    private float _lvl2DamageMod;
    private float _lvl2CooldownMod;

    [SerializeField]
    private float _lvl3AreaOfEffectRadiusMod;

    public override void UpgradeToLevel(int level)
    {
        var raven = (Raven)_weapon;
        switch (_level)
        {
            case 1:
                Activate();
                break;
            case 2:
                raven.BaseDamage *= _lvl2DamageMod;
                raven.BaseCooldown *= _lvl2CooldownMod;
                break;
            case 3:
                raven.AreaOfEffectRadius *= _lvl3AreaOfEffectRadiusMod;
                break;
        }
    }
}
