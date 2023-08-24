using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfish : ProjectileBase
{
    // properties true for all starfish
    public static int Damage;
    public static float BaseDamagePercentage = 0.9f;
    public static float StartingCooldown = 5f;
    public static float Cooldown = 5f;
    public static bool IsEnabled = false;
    
    public static float Duration = 5f;
    public static float DegreesPerFrame = 180f;
    public static bool IsActive = false;
    public static int AdditionalStarfish = 0;
    public static float TimeElapsedSinceLastStarfish;
    private static float _distanceOutsidePlayer = 2f;

    public float DegreesToNextStarfish; 
    public int identifier;
    private GameObject _player;
    private float _timeElapsedSinceActivated = 0.0f;

    void Start()
    {
        _player = GameObject.Find("Player");
        IsActive = true;

         // starting position
        transform.position = _player.transform.position + new Vector3(1f,0,0).normalized * _distanceOutsidePlayer;
        transform.RotateAround(_player.transform.position, Vector3.forward, DegreesToNextStarfish * identifier);
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
        if (level == 1)
        {
            IsEnabled = true;
            Damage = (int) (BaseDamage * BaseDamagePercentage);
            Cooldown = StartingCooldown * BaseCooldownPercentage;
            TimeElapsedSinceLastStarfish = Cooldown - 1f; // launch a starfish as soon as its enabled
        }
        else if (level == 2)
        {
            Duration *= 1.2f;
            BaseDamagePercentage = 1.2f;
            Damage = (int) (BaseDamage * BaseDamagePercentage);
        }
        else if (level == 3)
        {
            Duration *= 1.5f;
            StartingCooldown *= 0.7f;
            Cooldown = StartingCooldown * BaseCooldownPercentage;
        }
    }
}
