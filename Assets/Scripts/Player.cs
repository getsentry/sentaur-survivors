using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Projectile _projectilePrefab;

    [SerializeField] private int _hitPoints = 100;

    [SerializeField]
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

            // set the projectile's position to the player's position
            projectile.transform.position = transform.position;
        }
    }

    // a collision handler that is called when the enemy collides with another object
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
    }

    public void TakeDamage(int damage = 0)
    {
        _hitPoints -= damage;
        _hitPoints = Math.Max(_hitPoints, 0); // don't let the player have negative hit points

        _healthBar.SetHealth(_hitPoints / 100f);
        Debug.Log("Player.TakeDamage: Player took " + damage + " damage and now has " + _hitPoints + " hit points");
    }
}