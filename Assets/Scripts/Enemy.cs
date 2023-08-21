using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

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
    }
}
