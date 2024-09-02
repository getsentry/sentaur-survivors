using UnityEngine;

class SchnitzelUpgradePath : WeaponUpgradePath
{
    [SerializeField]
    private float _lvl2Scale;

    [SerializeField]
    private float _lvl3Scale;

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
