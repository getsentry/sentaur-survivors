using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much damage the enemy does to the player when they collide")]
    private int _collisionDamage = 10;

    [SerializeField]
    [Tooltip("How many hitpoints the enemy has")]
    private int _hitpoints = 20;

    [SerializeField]
    [Tooltip("How many points the player gets for killing this enemy")]
    private int _scoreValue = 10;

    [SerializeField]
    [Tooltip("The prefab to use for the damage text")]
    private DamageText _damageTextPrefab;

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

        player.TakeDamage(_collisionDamage);
    }

    public void TakeDamage(int damage) {
        _hitpoints -= damage;
        _hitpoints = Mathf.Max(_hitpoints, 0); // don't let the enemy have negative hit points

        Debug.Log("Enemy.TakeDamage: Enemy took " + damage + " damage and now has " + _hitpoints + " hit points");

        Debug.Log("enemy position is " + transform.position);
        // instantiate a DamageText prefab to show how much damage was done
        var damageText = Instantiate(
            _damageTextPrefab,   // prefab to instantiate
            transform.root,  // spawn at the enemy position 
            true // instantiate in world space
        );
        damageText.transform.position = new Vector2(transform.position.x, transform.position.y);

        // these should be the same position
        Debug.Log("enemy position is " + transform.position);
        Debug.Log("dmg text position is " + damageText.transform.position);

        // when these debug statements emit, they have the same value now -- but in the editor
        // (after they've been rendered) they don't have the same value
 

        damageText.SetDamage(damage);

        if (_hitpoints == 0) {

            // destroy the enemy
            Destroy(this.gameObject);

            EventManager.TriggerEvent("EnemyDestroyed", new EventData(_scoreValue));
        }
    }
}
