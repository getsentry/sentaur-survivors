using System.Collections;
using UnityEngine;

public class SpeedPickup : PickupBase
{
    [SerializeField]
    private int _newSpeed;

    protected override void OnCollect(Player player)
    {
        player.SpeedUp(_newSpeed, _effectDuration);
    }
}
