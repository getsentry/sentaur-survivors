using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected bool _isEnabled = false;

    [SerializeField]
    public float BaseCooldown;

    [SerializeField]
    public float BaseDamage;

    public float Cooldown => BaseCooldown * Player.Instance.WeaponManager.GlobalCooldownModifier;
    public int Damage => (int)(Player.Instance.WeaponManager.GlobalDamageModifier * BaseDamage);
    public int Count => Player.Instance.WeaponManager.GlobalCountModifier;

    protected float _timeElapsedSinceLastFire = 0.0f;

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
        ResetCooldown();
    }

    public virtual void ResetCooldown()
    {
        _timeElapsedSinceLastFire = 0.0f;
    }

    public void Enable()
    {
        _isEnabled = true;
    }
}
