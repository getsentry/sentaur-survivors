using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemy : Enemy
{
    override protected Vector2 DetermineDirection(GameObject player)
    {
        // get direction facing player
        Vector2 direction = Player.Instance.transform.position - transform.position;

        return Vector3.Normalize(direction);
    }


    override protected void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // ignores everything but player
        if (collision.gameObject.name != "Barrier")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
