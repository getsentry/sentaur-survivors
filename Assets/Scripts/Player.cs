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
    private float _projectileFireRate = 0.5f;

    private float timeElapsedSinceLastProjectile = 0.0f;

    private HealthBar _healthBar;

    // Start is called before the first frame update
    void Start()
    {
        // get a reference to the Healthbar component
        _healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * 5,
            transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * 5,
            transform.position.z
        );

        // fire a projectile once enough time has elapsed
        timeElapsedSinceLastProjectile += Time.deltaTime;
        if (timeElapsedSinceLastProjectile > _projectileFireRate)
        {
            timeElapsedSinceLastProjectile = 0.0f;

            // instantiate a new projectile
            var projectile = Instantiate(_projectilePrefab);

            // projectile moves in the direction of the current mouse cursor
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePosition - transform.position;
            projectile.SetDirection(direction);

            // set the projectile's position to the player's position + a little bit
            // outside the player in the direction of the mouse cursor
            var distanceOutsidePlayer = 2.0f;
            projectile.transform.position = transform.position + direction.normalized * distanceOutsidePlayer;
        }
    }

    public void TakeDamage(int damage = 0)
    {
        _hitPoints -= damage;
        _hitPoints = Math.Max(_hitPoints, 0); // don't let the player have negative hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);
    }

    public void HealDamage(int healAmount = 0) {
        _hitPoints += healAmount;
        _hitPoints = Math.Min(_hitPoints, 100); // don't let the player have more than 100 hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);
    }
}