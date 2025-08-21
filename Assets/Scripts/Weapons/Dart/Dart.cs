using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Dart : WeaponBase
{
    private InputAction _lookAction;
    private InputAction _mouseAction;
    
    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    public int RearFiringDartCount = 0; // NOTE: rear-shooting darts

    [SerializeField]
    private float _spawnDistanceOutsidePlayer = 1.25f;

    [SerializeField]
    private float _shootingInterval = 0.4f; // time between consecutive darts

    [SerializeField]
    private float _areaOfEffectRadius = 0.25f;

    private bool _isShooting = false;
    private GameObject _player;

    [SerializeField]
    private DartProjectile _dartProjectilePrefab;

    private Vector3 _shootingDirection = Vector3.right;
    private Arrow _arrow;

    // Override mechanism for external shooting direction control
    private bool _hasExternalShootingDirection = false;
    private Vector3 _externalShootingDirection;

    private void Awake()
    {
        _lookAction = InputSystem.actions.FindAction("Look");
        _mouseAction = InputSystem.actions.FindAction("Mouse");
        
        _arrow = Player.Instance.GetComponentInChildren<Arrow>();
    }
    
    public void Start()
    {
        _isEnabled = true;
        _player = Player.Instance.gameObject;
    }

    protected override void Update()
    {
        base.Update();

        // Only calculate direction if not overridden externally
        if (!_hasExternalShootingDirection)
        {
            _shootingDirection = CalculateDirection(_player);
        }
        else
        {
            _shootingDirection = _externalShootingDirection;
        }
        
        // If we have a valid direction vector
        if (_shootingDirection != Vector3.zero)
        {
            // Create a rotation where the arrow's right vector points in the direction
            // This works better for 2D objects that should point in a direction
            float angle = Mathf.Atan2(_shootingDirection.y, _shootingDirection.x) * Mathf.Rad2Deg;
            if (_arrow != null)
            {
                _arrow.transform.rotation = Quaternion.Euler(0, 0, angle);    
            }
        }
    }

    /// <summary>
    /// Sets the shooting direction externally, overriding the normal input-based calculation
    /// </summary>
    /// <param name="direction">The direction to shoot in</param>
    public void SetShootingDirection(Vector3 direction)
    {
        _hasExternalShootingDirection = true;
        _externalShootingDirection = direction.normalized;
    }

    /// <summary>
    /// Clears the external shooting direction override, returning to normal input-based calculation
    /// </summary>
    public void ClearShootingDirectionOverride()
    {
        _hasExternalShootingDirection = false;
    }

    public override void Fire()
    {
        base.Fire();

        if (_isShooting)
        {
            // if the dart is already firing, exit early (we only start counting
            // after the dart has CEASED firing)
            return;
        }

        StartCoroutine(ShootDarts());
    }

    public IEnumerator ShootDarts()
    {
        _isShooting = true;
        
        // shoot the base number of darts
        for (int i = 0; i < Count; i++)
        {
            ShootADart(_dartProjectilePrefab, _player, _shootingDirection);
            if (RearFiringDartCount > i)
            {
                ShootADart(_dartProjectilePrefab, _player, _shootingDirection * -1);
            }

            yield return new WaitForSeconds(_shootingInterval);
        }

        // accounting for case where # of backwards darts > # of forwards darts
        int remainingDarts = RearFiringDartCount - Count;
        if (remainingDarts > 0)
        {
            _shootingDirection *= -1;
            for (int i = 0; i < remainingDarts; i++)
            {
                ShootADart(_dartProjectilePrefab, _player, _shootingDirection);
                yield return new WaitForSeconds(_shootingInterval);
            }
        }

        // reset cooldown after all darts have been shot
        ResetCooldown();

        _isShooting = false;
        yield return null;
    }

    private void ShootADart(DartProjectile prefab, GameObject player, Vector3 direction)
    {
        DartProjectile dart = Instantiate(prefab);
        dart.Initialize(Damage, _speed, _areaOfEffectRadius);
        dart.transform.parent = player.transform.parent;

        // initial position
        dart.transform.position =
            player.transform.position + direction.normalized * _spawnDistanceOutsidePlayer;

        dart.SetDirection(direction);
    }

    private Vector3 CalculateDirection(GameObject player)
    {
        if (Gamepad.current != null)
        {
            var direction = _lookAction.ReadValue<Vector2>(); 
            if (direction.magnitude < 0.1f)
            {
                // Don't change it if we're not aiming
                return _shootingDirection;
            }

            return direction;
        }
        
        var mousePosition = Camera.main.ScreenToWorldPoint(_mouseAction.ReadValue<Vector2>());
        var targetDirection = mousePosition - player.transform.position;
        
        return targetDirection;
    }
}
