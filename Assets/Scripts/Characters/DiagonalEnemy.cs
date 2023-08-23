using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiagonalEnemy : Enemy
{
    Vector2 _direction = new Vector3(0.5f, 0.5f, 0);

    override protected Vector2 DetermineDirection(GameObject player) {
        var nextPosition = (Vector2)transform.position + _direction * Time.deltaTime * _speed;
        return nextPosition;
    }

    override protected void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Barrier") {
            // rotate direction 90 degrees
            _direction = Quaternion.AngleAxis(-90, Vector3.forward) * _direction;
        }
    }
}
