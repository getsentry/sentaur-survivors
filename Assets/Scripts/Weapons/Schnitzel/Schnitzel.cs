using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Schnitzel : WeaponBase
{
    private InputAction _lookAction;
    private InputAction _mouseAction;
    
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private float _spawnDistanceOutsidePlayer = 1.25f;

    [SerializeField]
    private float _shootingInterval = 0.25f; // time between consecutive schnitzel

    [SerializeField]
    public float Scale = 1.0f;

    [SerializeField]
    private SchnitzelProjectile _schnitzelProjectilePrefab;

    private Vector3 _shootingDirection = Vector3.right;
    
    private void Awake()
    {
        _lookAction = InputSystem.actions.FindAction("Look");
        _mouseAction = InputSystem.actions.FindAction("Mouse");
    }

    public override void Fire()
    {
        base.Fire();

        var player = Player.Instance.gameObject;
        StartCoroutine(ShootSchnitzels(player));
    }

    public IEnumerator ShootSchnitzels(GameObject player)
    {
        SchnitzelProjectile schnitzelProjectilePrefab = _schnitzelProjectilePrefab;
        _shootingDirection = CalculateDirection(player);

        // shoot the base number of schnitzel
        for (int i = 0; i < Count; i++)
        {
            ShootOneSchnitzel(schnitzelProjectilePrefab, player, _shootingDirection);

            yield return new WaitForSeconds(_shootingInterval);
            // get new updates mouse coords inbetween shots
            _shootingDirection = CalculateDirection(player);
        }

        // reset cooldown after all schnitzels fired
        ResetCooldown();

        yield return null;
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

    private void ShootOneSchnitzel(SchnitzelProjectile prefab, GameObject player, Vector3 direction)
    {
        SchnitzelProjectile schnitzel = Instantiate(prefab);
        schnitzel.Initialize(Damage, _speed);
        schnitzel.transform.parent = player.transform.parent;

        // initial position
        schnitzel.transform.position =
            player.transform.position + direction.normalized * _spawnDistanceOutsidePlayer;

        // adjust scale
        schnitzel.transform.localScale = new Vector3(Scale, Scale, Scale);

        schnitzel.SetDirection(direction);
    }
}
