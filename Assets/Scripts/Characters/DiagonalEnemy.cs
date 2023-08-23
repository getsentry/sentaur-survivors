using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiagonalEnemy : Enemy
{
    Vector2 _direction = new Vector3(0.5f, 0.5f, 0);

    override protected Vector2 DetermineDirection(GameObject player) {
        return Vector3.Normalize(_direction);
    }

    override protected void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Barrier") {
            // rotate direction 90 degrees
            _direction = Quaternion.AngleAxis(-90, Vector3.forward) * _direction;
        } else if (collision.gameObject.tag == "Enemy") {
            // ignore physics collision with other enemies
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
