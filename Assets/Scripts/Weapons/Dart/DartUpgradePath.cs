using UnityEngine;

public class DartUpgradePath : WeaponUpgradePath
{
    [SerializeField]
    private int _lvl2RearFiringDartCount = 1;

    [SerializeField]
    private float _lvl3DamageMod = 1.6f;

    public override void UpgradeToLevel(int level)
    {
        var dart = (Dart)_weapon;
        switch (_level)
        {
            case 1:
                Activate();
                break;
            case 2:
                dart.RearFiringDartCount = _lvl2RearFiringDartCount;
                break;
            case 3:
                dart.BaseDamage *= _lvl3DamageMod;
                break;
        }
    }
}
