using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfish : MonoBehaviour
{
    // properties true for all starfish
    public static int Damage = 5;
    public static float FireRate = 8f;
    public static float Duration = 5f;
    public static float DegreesPerFrame = 180f;
    private static float _distanceOutsidePlayer = 2f;

    private GameObject _player;
    private float _timeElapsedSinceActivated = 0.0f;
    private Vector3 _positionOffset;

    void Awake()
    {
        _player = GameObject.Find("Player");
    }

    void Start()
    {
        // starting position
        transform.position = _player.transform.position + new Vector3(1f,1f,0).normalized * _distanceOutsidePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsedSinceActivated += Time.deltaTime;
        if (_timeElapsedSinceActivated > Duration)
        {
            Destroy(gameObject);
        }
        
    }

    void LateUpdate()
    {
        // move where player is
        _positionOffset = transform.position - _player.transform.position; 
        transform.position = _player.transform.position + _positionOffset.normalized * _distanceOutsidePlayer;
        transform.RotateAround(_player.transform.position, Vector3.forward, DegreesPerFrame * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if the starfish collides with an enemy, damage the enemy
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            DamageEnemy(enemy);
        } 
    }

    // Deal damage to the enemy because they were hit by a dart
    private void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }
}
