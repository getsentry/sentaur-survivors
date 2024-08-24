using System.Collections;
using UnityEngine;

public class SpeedPickup : PickupBase
{
    [SerializeField]
    private int _newSpeed;

    protected override void OnCollect(Player player)
    {
        player.ApplySpeedUp(_newSpeed, _effectDuration);
    }
}
