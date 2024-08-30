using Unity.VisualScripting;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected bool _isEnabled = false;

    [SerializeField]
    protected float _startingCooldown;

    [SerializeField]
    public float _baseDamagePercentage = 0.8f;

    public float Cooldown => _startingCooldown * _baseCooldownPercentage;
    public int Damage => (int)(_baseDamage * _baseDamagePercentage);

    protected float _timeElapsedSinceLastFire = 0.0f;

    // proxied
    protected int _baseDamage => _weaponManager.BaseDamage;
    protected float _baseCooldownPercentage => _weaponManager.BaseCooldownPercentage;
    protected int _baseCount => _weaponManager.BaseCount;

    protected WeaponManager _weaponManager;

    private void Awake()
    {
        _weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
    }

    protected virtual void Update()
    {
        if (_isEnabled)
        {
            _timeElapsedSinceLastFire += Time.deltaTime;
        }
    }

    public virtual bool CanFire()
    {
        return _isEnabled && _timeElapsedSinceLastFire >= Cooldown;
    }

    protected virtual void Fire()
    {
        _timeElapsedSinceLastFire = 0.0f;
    }
}
