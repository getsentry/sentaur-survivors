using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The projectile prefab that the player fires")]
    private Projectile _projectilePrefab;

    [SerializeField] 
    [Tooltip("How many hit points the player has")]
    private int _hitPoints = 100;

    [SerializeField]
    [Tooltip("The maximum number of hit points the player can have")]
    private int _maxHitPoints = 100;

    [SerializeField]
    [Tooltip("How frequently the player fires projectiles (in seconds)")]
    private float _projectileFireRate = 0.2f;

    [SerializeField]
    [Tooltip("The number of projectiles fired by the player at once")]
    private int _projectileCount = 1;

    [SerializeField]
    [Tooltip("How fast the player moves (how exaclty I don't know)")]
    private float _playerMoveRate = 5;

    private float _timeElapsedSinceLastProjectile = 0.0f;

    // the further a projectile's path is rotated from the center projectile
    private int _spreadInDegrees = 0; 

    private HealthBar _healthBar;
    public Animator animator;
    Vector2 movement;
    bool facingRight = false;

    public AudioSource takeDamageSound;

    // Start is called before the first frame update
    void Start()
    {
        // get a reference to the Healthbar component
        _healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        takeDamageSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(
            transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * _playerMoveRate,
            transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * _playerMoveRate
        );

        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x > 0) {
            facingRight = true;
        } else if (movement.x < 0) {
            facingRight = false;
        } // if 0, don't modify
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("FacingRight", facingRight);

        // fire a projectile once enough time has elapsed
        _timeElapsedSinceLastProjectile += Time.deltaTime;

        if (_timeElapsedSinceLastProjectile > _projectileFireRate)
        {
            _timeElapsedSinceLastProjectile = 0.0f;

            float degreesToRotate = _spreadInDegrees;
            int numberOfProjectilePairs = _projectileCount / 2;

            // instantiate new projectile(s), starting from the two furthest projectiles then going inward
            for (int i = 0; i < numberOfProjectilePairs ; i++) 
            {
                for (int j = 0; j < 2; j++) 
                {
                    var projectile = Instantiate(_projectilePrefab);
                    projectile.transform.parent = transform.parent;
                    Vector3 direction = CalculateProjectileDirection();
                    direction = RotateDirection(direction, degreesToRotate);
                    degreesToRotate *= -1; // to get the proper rotation for the next projectile, which is on the other side of the center projectile
                    projectile.SetDirection(direction);

                    // set the projectile's position to the player's position + a little bit
                    // outside the player in the direction of the mouse cursor
                    var distanceOutsidePlayer = 2.0f;
                    projectile.transform.position = transform.position + direction.normalized * distanceOutsidePlayer;
                }

                degreesToRotate -= (degreesToRotate / numberOfProjectilePairs);
            }

            if (_projectileCount % 2 != 0) 
            {
                // odd number of projectiles, so we need to create a center projectile
                var projectile = Instantiate(_projectilePrefab);
                projectile.transform.parent = transform.parent;
                Vector3 direction = CalculateProjectileDirection();
                direction = RotateDirection(direction, 0);
                projectile.SetDirection(direction);

                // set the projectile's position to the player's position + a little bit
                // outside the player in the direction of the mouse cursor
                var distanceOutsidePlayer = 2.0f;
                projectile.transform.position = transform.position + direction.normalized * distanceOutsidePlayer;
            }
        }
    }

    private Vector3 CalculateProjectileDirection() 
    {
        // projectile moves in the direction of the current mouse cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;

        return direction;
    }

    private Vector3 RotateDirection(Vector3 direction, float degreesToRotate)
    {
        float degreesInRadians = degreesToRotate * Mathf.Deg2Rad;
        return new Vector3(
            direction.x * Mathf.Cos(degreesInRadians) - direction.y * Mathf.Sin(degreesInRadians),
            direction.x * Mathf.Sin(degreesInRadians) + direction.y * Mathf.Cos(degreesInRadians) 
        );
    }

    public void TakeDamage(int damage = 0)
    {
        _hitPoints -= damage;
        _hitPoints = Math.Max(_hitPoints, 0); // don't let the player have negative hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);

        if (_hitPoints == 0)
        {
            // emit player death event
            EventManager.TriggerEvent("PlayerDeath");
        } else {
            // play damage sound effect
            takeDamageSound.Play();
        }
    }

    public void HealDamage(int healAmount = 0) {
        _hitPoints += healAmount;
        _hitPoints = Math.Min(_hitPoints, 100); // don't let the player have more than 100 hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);
    }

    public void UpgradeCount(int level) 
    {
        _projectileCount+= 2;
        _spreadInDegrees += 10;
        Debug.Log("upgrade count to " + _projectileCount);
    }

    public void UpgradeSpeed(int level) 
    {
        _projectileFireRate *= 0.5f;
    }

    public void UpgradeDamage(int level) 
    {
        Projectile.Damage *= 2;
    }
}