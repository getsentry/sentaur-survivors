using Unity.VisualScripting;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected bool _isEnabled = false;

    [SerializeField]
    protected float _startingCooldown;

    [SerializeField]
    public float _baseDamage;

    public float Cooldown => _startingCooldown * _weaponManager.GlobalCooldownModifier;
    public int Damage => (int)(_weaponManager.GlobalDamageModifier * _baseDamage);
    public int Count => _weaponManager.GlobalCountModifier;

    protected float _timeElapsedSinceLastFire = 0.0f;

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

    public virtual void Fire()
    {
        _timeElapsedSinceLastFire = 0.0f;
    }
}
