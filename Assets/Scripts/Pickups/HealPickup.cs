using UnityEngine;

public class HealPickup : PickupBase
{
    [SerializeField]
    private int _healAmount;

    protected override void OnCollect(Player player)
    {
        player.ApplyHeal(_healAmount);
    }

    protected override string GetEffectText()
    {
        return $"+{_healAmount} HP!";
    }
}
