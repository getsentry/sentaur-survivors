using UnityEngine;

class StarfishUpgradePath : WeaponUpgradePath
{
    [SerializeField]
    private float _lvl2DurationMod = 1.2f;

    [SerializeField]
    private float _lvl3DurationMod = 1.5f;
    private float _lvl3CooldownMod = 0.7f;

    public override void UpgradeToLevel(int level)
    {
        var starfish = (Starfish)_weapon;
        switch (_level)
        {
            case 1:
                Activate();
                break;
            case 2:
                starfish.Duration *= _lvl2DurationMod;
                break;
            case 3:
                starfish.Duration *= _lvl3DurationMod;
                starfish.BaseCooldown *= _lvl3CooldownMod;
                break;
        }
    }
}
