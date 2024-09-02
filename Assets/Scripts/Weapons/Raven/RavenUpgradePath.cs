using UnityEngine;

public class RavenUpgradePath : WeaponUpgradePath
{
    [SerializeField]
    private string[] _description = new string[]
    {
        "heat-seeking bomb targets closest enemy",
        "+33% damage and +20% cooldown!",
        "+60% area of effect"
    };
    protected override string[] Descriptions => _description;

    [SerializeField]
    private float _lvl2DamageMod = 1.33f;

    [SerializeField]
    private float _lvl2CooldownMod = 0.8f;

    [SerializeField]
    private float _lvl3AreaOfEffectRadiusMod = 1.5f;

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
