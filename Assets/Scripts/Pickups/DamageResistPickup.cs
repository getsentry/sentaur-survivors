using UnityEngine;

public class DamageResistPickup : PickupBase
{
    [SerializeField]
    private float _damageReduction;

    protected override void OnCollect(Player player)
    {
        player.ReduceDamage(_damageReduction, _effectDuration);
    }
}
