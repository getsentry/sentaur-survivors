using UnityEngine;

public class NotHotDogPickup : PickupBase
{
    [SerializeField] private int _damageAmount = 50;
    [SerializeField] private GameObject _effectPrefab;
    
    private DemoConfiguration _demoConfig;
    
    private void Awake()
    {
        _demoConfig = Resources.Load("DemoConfig") as DemoConfiguration;
    }

    protected override void OnCollect(Player player)
    {
        base.OnCollect(player);
        
        player.ApplyDamage(_damageAmount);

        if (_demoConfig != null && _demoConfig.NotHotDogParticleEffect)
        {
            var effect = Instantiate(_effectPrefab);
            effect.transform.position = transform.position;    
        }
    }
}
