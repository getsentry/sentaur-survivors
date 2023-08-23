using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfish : ProjectileBase
{
    // properties true for all starfish
    public static int Damage;
    public static float BaseDamagePercentage = 1.2f;
    public static float Cooldown;
    public static float BaseCooldownPercentage = 4.5f;
    
    public static float Duration = 5f;
    public static float DegreesPerFrame = 180f;
    public static bool IsActive = false;
    public static int AdditionalStarfish = 0;
    private static float _distanceOutsidePlayer = 2f;

    private GameObject _player;
    private float _timeElapsedSinceActivated = 0.0f;

    void Start()
    {
        _player = GameObject.Find("Player");
        IsActive = true;

        // starting position
        transform.position = _player.transform.position + new Vector3(1f,0,0).normalized * _distanceOutsidePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsedSinceActivated += Time.deltaTime;
        if (_timeElapsedSinceActivated > Duration)
        {
            IsActive = false;
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        // move the same amount the player moved in this frame
        Vector3 playerMovement = _player.transform.position - _player.GetComponent<Player>().lastPosition;
        transform.position += playerMovement;
        transform.RotateAround(_player.transform.position, Vector3.forward, DegreesPerFrame * Time.deltaTime);
    }

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        // if the starfish collides with an enemy, damage the enemy
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
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
        enemy.TakeDamage(Damage);
    }

    public static void UpgradeStarfish(int level)
    {
        if (level == 2)
        {
            Duration *= 1.5f;
        }
        else if (level == 3)
        {
            Duration *= 2;
            BaseCooldownPercentage = 3f;
            Cooldown = BaseCooldown * BaseCooldownPercentage;
        }
    }
}
