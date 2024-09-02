using UnityEngine;

public class RavenUpgradePath : WeaponUpgradePath
{
    [SerializeField]
    private float _lvl2DamageMod = 1.33f;

    [SerializeField]
    private float _lvl3AreaOfEffectRadiusMod = 1.6f;

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
                break;
            case 3:
                raven.AreaOfEffectRadius *= _lvl3AreaOfEffectRadiusMod;
                break;
        }
    }
}
