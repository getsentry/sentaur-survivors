
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("How fast the projectile moves")]
    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    [Tooltip("How much damage this projectile does to an enemy")]
    private int _damage = 10;

    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Projectile.OnCollisionEnter2D: " + other.gameObject.name);
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
        enemy.TakeDamage(_damage);
    }
}
