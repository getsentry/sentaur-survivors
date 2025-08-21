using System.Collections;
using UnityEngine;

public class BeerbootPickup : PickupBase
{
    [SerializeField] private float _cooldownModifier = 1.0f;

    protected override void OnCollect(Player player)
    {
        var currentEffectCooldownModifier = Player.Instance.WeaponManager.GlobalEffektCooldownModifier;
        Player.Instance.WeaponManager.GlobalEffektCooldownModifier = _cooldownModifier;

        StartCoroutine(ResetEffectCooldown(currentEffectCooldownModifier));
    }

    private IEnumerator ResetEffectCooldown(float cooldownModifier)
    {
        yield return new WaitForSeconds(_effectDuration);

        Player.Instance.WeaponManager.GlobalEffektCooldownModifier = cooldownModifier;
    }

    protected override string GetEffectText()
    {
        return $"+{_cooldownModifier}x speed!";
    }
}
