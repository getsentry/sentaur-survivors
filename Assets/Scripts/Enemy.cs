using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _damageToPlayer = 10; // unused, but just an example of how this could be set/accessed

    [SerializeField]
    private int _hitpoints = 20;

    // Update is called once per frame
    void Update()
    {
        // move towards the player character
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position,
                Time.deltaTime
            );
        }
    }
    
    // a collision handler that is called when the enemy collides with another object
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        // if the enemy collides with the player, destroy the player
        if (collision.gameObject.name == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
            DamagePlayer(player);

            // Destroy the enemy (for now they explode if they touch the player)
            Destroy(this.gameObject);
        }
    }
    
    // Deal damage to the player because they touched
    private void DamagePlayer(Player player) {
        Debug.Log("Enemy.DamagePlayer: Player was damaged by " + gameObject.name);

        player.TakeDamage(_damageToPlayer);
    }

    public void TakeDamage(int damage) {
        _hitpoints -= damage;
        _hitpoints = Mathf.Max(_hitpoints, 0); // don't let the enemy have negative hit points

        Debug.Log("Enemy.TakeDamage: Enemy took " + damage + " damage and now has " + _hitpoints + " hit points");

        if (_hitpoints == 0) {
            // destroy the enemy
            Destroy(this.gameObject);
        }
    }
}
