using UnityEngine;

class StarfishUpgradePath : WeaponUpgradePath
{
    [SerializeField]
    private string[] _description = new string[]
    {
        "orbits around you, wreaking havoc",
        "+40% orbit duration",
        "+30% orbit duration and -30% cooldown!"
    };
    protected override string[] Descriptions => _description;

    [SerializeField]
    private float _lvl2DurationMod = 1.4f;

    [SerializeField]
    private float _lvl3DurationMod = 1.3f;
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
