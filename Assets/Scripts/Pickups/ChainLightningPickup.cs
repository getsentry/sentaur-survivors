using UnityEngine;

public class ChainLightningPickup : PickupBase
{
    [SerializeField]
    [Tooltip("Prefab of the hovering effect (ChainLightningEffect).")]
    private GameObject _effectPrefab;

    [SerializeField]
    [Tooltip("Damage per lightning strike per enemy.")]
    private int _damage = 15;

    [SerializeField]
    [Tooltip("Seconds between each chain lightning strike.")]
    private float _fireInterval = 1.5f;

    [SerializeField]
    [Tooltip("Max number of enemies hit per strike (chain length).")]
    private int _chainCount = 4;

    [SerializeField]
    [Tooltip("Radius to detect closest enemy from player.")]
    private float _detectRadius = 12f;

    [SerializeField]
    [Tooltip("Radius for lightning to jump to next enemy from last struck.")]
    private float _chainJumpRange = 5f;

    protected override void OnCollect(Player player)
    {
        var existing = player.GetComponentInChildren<ChainLightningEffect>();
        if (existing != null)
        {
            existing.ExtendDuration(_effectDuration);
            return;
        }

        if (_effectPrefab == null)
            return;

        GameObject effectInstance = Instantiate(_effectPrefab, player.transform);
        effectInstance.transform.localPosition = new Vector3(0, 1.2f, 0);

        var effect = effectInstance.GetComponent<ChainLightningEffect>();
        if (effect != null)
        {
            effect.Initialize(
                _effectDuration,
                _damage,
                _fireInterval,
                _chainCount,
                _detectRadius,
                _chainJumpRange
            );
        }
    }

    protected override string GetEffectText()
    {
        return "Seer sees all!";
    }
}
