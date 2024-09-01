using UnityEngine;

public class RavenUpgrades : UpgradeBase
{
    [SerializeField]
    private float _lvl2DamageMod = 1.33f;

    [SerializeField]
    private float _lvl3AreaOfEffectRadiusMod = 1.6f;

    public override void UpgradeToLevel(int level)
    {
        // get raven
        var raven = FindObjectOfType<Raven>();
        switch (_currentLevel)
        {
            case 1:
                raven.Enable();
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
