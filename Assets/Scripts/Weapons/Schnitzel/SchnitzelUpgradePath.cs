using UnityEngine;

class SchnitzelUpgradePath : WeaponUpgradePath
{
    [SerializeField]
    private string[] _description = new string[]
    {
        "it's like an axe",
        "+40% area of effect",
        "+30% area of effect"
    };
    protected override string[] Descriptions => _description;

    [SerializeField]
    private float _lvl2Scale = 1.4f;

    [SerializeField]
    private float _lvl3Scale = 1.3f;

    public override void UpgradeToLevel(int level)
    {
        var schnitzel = (Schnitzel)_weapon;
        switch (_level)
        {
            case 1:
                Activate();
                break;
            case 2:
                schnitzel.Scale *= _lvl2Scale;
                break;
            case 3:
                schnitzel.Scale *= _lvl3Scale;
                break;
        }
    }
}
