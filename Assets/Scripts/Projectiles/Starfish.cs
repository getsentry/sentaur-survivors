using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfish : ProjectileBase
{
    // properties true for all starfish
    public static int Damage;
    public static float BaseDamagePercentage = 1.2f;
    public static float Cooldown;
    public static float BaseCooldownPercentage = 1f; // change back to 5
    public static bool IsEnabled = false;
    
    public static float Duration = 5f;
    public static float DegreesPerFrame = 180f;
    public static bool IsActive = false;
    public static int AdditionalStarfish = 0;
    public static float TimeElapsedSinceLastStarfish;
    private static float _distanceOutsidePlayer = 1.5f;

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
        // reposition the starfish so it still looks the same distance to the player regardless of how they moved; TODO: still wonky
        Vector3 targetPosition = _player.transform.position + (_player.transform.position - transform.position).normalized * _distanceOutsidePlayer;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, -(_distanceOutsidePlayer - Vector3.Distance(transform.position, _player.transform.position)));
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
            Cooldown = BaseCooldown * BaseCooldownPercentage;
            TimeElapsedSinceLastStarfish = Cooldown - 1f; // launch a starfish as soon as its enabled
        }
        else if (level == 2)
        {
            Duration *= 1.5f;
        }
        else if (level == 3)
        {
            Duration *= 2;
            BaseCooldownPercentage = 3.5f;
            Cooldown = BaseCooldown * BaseCooldownPercentage;
        }
    }
}
