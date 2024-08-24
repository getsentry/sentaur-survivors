using UnityEngine;

public class HealPickup : PickupBase
{
    [SerializeField]
    private int _healAmount;

    protected override void OnCollect(Player player)
    {
        player.HealDamage(_healAmount);
    }
}
