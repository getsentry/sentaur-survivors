using System.Collections;
using UnityEngine;

public class SpeedPickup : PickupBase
{
    [SerializeField]
    private int _speedMultiple;

    protected override void OnCollect(Player player)
    {
        player.ApplySpeedUp(_speedMultiple, _effectDuration);
    }

    protected override string GetEffectText()
    {
        return $"+{_speedMultiple}x speed!";
    }
}
