using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfish : ProjectileBase
{
    // properties true for all starfish
    public static int Damage;
    public static float BaseDamagePercentage = 1.2f;
    public static float Cooldown;
    public static float BaseCooldownPercentage = 5f;
    public static bool IsEnabled = false;
    
    public static float Duration = 5f;
    public static float DegreesPerFrame = 180f;
    public static bool IsActive = false;
    public static int AdditionalStarfish = 0;
    private static float _distanceOutsidePlayer = 1f;

    public int identifier; // TODO: use this to space out starfish
    private GameObject _player;
    private float _timeElapsedSinceActivated = 0.0f;

    void Start()
    {
        _player = GameObject.Find("Player");
        IsActive = true;

        // TODO: make this better lol (this is used to make sure the starfish don't all start in the same spot if there are multiple)
        Vector3 positionRelativeToPlayer = new Vector3(0,0,0);
        switch (identifier)
        {
            case 0: 
                positionRelativeToPlayer.x = 1f;
                break;
            case 1: 
                positionRelativeToPlayer.x = 1f;
                positionRelativeToPlayer.y = 1f;
                break;
            case 2: 
                positionRelativeToPlayer.y = 1f;
                break;
            case 3: 
                positionRelativeToPlayer.x = -1f;
                positionRelativeToPlayer.y = 1f;
                break;
            case 4: 
                positionRelativeToPlayer.x = -1f;
                break;
            case 5: 
                positionRelativeToPlayer.x = -1f;
                positionRelativeToPlayer.y = -1f;
                break;
            case 6: 
                positionRelativeToPlayer.x = -1f;
                break;
            case 7: 
                positionRelativeToPlayer.x = 1f;
                positionRelativeToPlayer.y = -1f;
                break;
        }

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
        if (level == 1)
        {
            IsEnabled = true;
            Damage = (int) (BaseDamage * BaseDamagePercentage);
            Cooldown = BaseCooldown * BaseCooldownPercentage;
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
