
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("How fast the projectile moves")]
    [SerializeField]
    private float _speed = 20.0f;

    [SerializeField]
    [Tooltip("How much damage this projectile does to an enemy")]
    public static int Damage = 10;

    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        // rotate the projectile to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if the projectile collides with an enemy, destroy the enemy
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            DamageEnemy(enemy);

            // Destroy the projectile
            Destroy(this.gameObject);
        }
    }

    // Deal damage to the enemy because they were hit by a projectile
    private void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    } 
}
