using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiagonalEnemy : Enemy
{
    Vector2 _direction;

    void Awake() {
        base.Awake();
        
        // initialize a random direction
        _direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

    }
    override protected Vector2 DetermineDirection(GameObject player)
    {
        return Vector3.Normalize(_direction);
    }

    override protected void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Barrier")
        {
            // reflect the velocity of the enemy off the barrier
            // https://stackoverflow.com/questions/49790711/reflect-a-projectile-on-collision-in-unity

            _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);
            Debug.Log("Enemy " + this.name + " has collision.contacts[0].normal: " + collision.contacts[0].normal);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            // ignore physics collision with other enemies
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
