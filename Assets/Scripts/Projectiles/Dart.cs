
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Dart : MonoBehaviour
{
    // properties true for all darts
    public static float Speed = 10.0f;
    public static int Damage = 10;
    public static float FireRate = 2f;

    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * Speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        // rotate the dart to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the dart collides with an enemy, destroy the enemy
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            DamageEnemy(enemy);

            // Destroy the dart
            Destroy(this.gameObject);
        } else if (other.gameObject.tag == "Barrier") {
            // Destroy the dart
            Destroy(this.gameObject);
        }
    }

    // Deal damage to the enemy because they were hit by a dart
    private void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    } 
}
