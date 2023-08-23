
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {

    protected Rigidbody2D _rigidbody2D;
    public static int BaseDamage = 10;

    protected void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Hitbox") {
            var hitbox = other.gameObject.GetComponent<Hitbox>();
            var enemy = hitbox.Enemy;

            DamageEnemy(enemy);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Barrier") {
            // Destroy the dart
            Destroy(this.gameObject);
        }
    }

    // Deal damage to the enemy because they were hit by a dart
    virtual protected void DamageEnemy(Enemy enemy) {}

    public static void UpgradeDamage(int level)
    {
        ProjectileBase.BaseDamage *= 2;
    }
}