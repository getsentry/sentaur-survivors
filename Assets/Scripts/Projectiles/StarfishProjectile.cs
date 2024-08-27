using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishProjectile : ProjectileBase
{
    private int _damage;
    private float _duration;
    private float _degrees;

    private static float _distanceOutsidePlayer = 2f;
    public static float DegreesPerFrame = 180f;

    public float DegreesToNextStarfish;
    public int identifier;
    private GameObject _player;
    private float _timeElapsedSinceActivated = 0.0f;

    public void Initialize(int damage, float duration, float degrees)
    {
        _damage = damage;
        _duration = duration;
        _degrees = degrees;

        Debug.Log(
            "Starfish initialized with damage: "
                + _damage
                + ", degrees: "
                + _degrees
                + ", duration: "
                + _duration
        );
    }

    void Start()
    {
        _player = GameObject.Find("Player");

        // starting position
        transform.position =
            _player.transform.position + new Vector3(1f, 0, 0).normalized * _distanceOutsidePlayer;

        Debug.Log("Starfish starting position: " + transform.position);
        transform.RotateAround(_player.transform.position, Vector3.forward, _degrees * identifier);
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsedSinceActivated += Time.deltaTime;
        if (_timeElapsedSinceActivated > _duration)
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        transform.RotateAround(
            _player.transform.position,
            Vector3.forward,
            DegreesPerFrame * Time.deltaTime
        );
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // if the starfish collides with an enemy, damage the enemy
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<Enemy>();

            SoundManager.Instance.PlayHitSound();

            DamageEnemy(enemy);
        }
        else if (other.gameObject.tag == "Barrier")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other);
        }
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(_damage);
    }
}
