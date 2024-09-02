using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected bool _isEnabled = false;

    [SerializeField]
    public float BaseCooldown;

    [SerializeField]
    public float BaseDamage;

    public float Cooldown => BaseCooldown * _weaponManager.GlobalCooldownModifier;
    public int Damage => (int)(_weaponManager.GlobalDamageModifier * BaseDamage);
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

    public void Enable()
    {
        _isEnabled = true;
    }
}
