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
    public int hitpoints = 20;

    [SerializeField]
    [Tooltip("How many points the player gets for killing this enemy")]
    private int _scoreValue = 10;

    [SerializeField]
    [Tooltip("How much XP the player earns")]
    private int _xpValue = 10;
    
    [SerializeField]
    protected float _speed = 1f;

    [SerializeField]
    [Tooltip("The prefab to use for the damage text")]
    private DamageText _damageTextPrefab;

    [SerializeField]
    private XpDrop _xpDropPrefab;

    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rigidbody2D;

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        // move towards the player character
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            var direction = DetermineDirection(player);
            _rigidbody2D.velocity = direction * _speed;

            // flip sprite in x direction if moving left
            _spriteRenderer.flipX = direction.x < 0;
        }
    }

    /**
     * Returns a normalized vector in the direction of the desired movement
     */
    virtual protected Vector2 DetermineDirection(GameObject player) {
        return Vector3.Normalize(player.transform.position - transform.position);
    }
    
    // a collision handler that is called when the enemy collides with another object
    virtual protected void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        // if the enemy collides with the player, destroy the player
        if (collision.gameObject.name == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
            DamagePlayer(player);

            // Destroy the enemy (for now they explode if they touch the player)
            Death();
            
        }
    }
    
    virtual public void Death(bool leaveXp = false) {

        if (leaveXp) {
            // instantiate an xp drop at this position
            var xpDrop = Instantiate(_xpDropPrefab, transform.position, Quaternion.identity);
            xpDrop.SetXp(_xpValue);
        }

        Destroy(this.gameObject);
    }

    // Deal damage to the player because they touched
    private void DamagePlayer(Player player) {
        Debug.Log("Enemy.DamagePlayer: Player was damaged by " + gameObject.name);

        player.TakeDamage(_collisionDamage);
    }

    public void TakeDamage(int damage) {
        hitpoints -= damage;
        hitpoints = Mathf.Max(hitpoints, 0); // don't let the enemy have negative hit points

        // spawn the damage text above the enemy by 1 unit (32px)
        var damageTextPosition = new Vector2(
            transform.position.x,
            transform.position.y + 1.0f
        );
        _damageTextPrefab.Spawn(transform.root, damageTextPosition, damage);

        if (hitpoints == 0) {

            Death(leaveXp: true);

            EventManager.TriggerEvent("EnemyDestroyed", new EventData(_scoreValue));
        }
    }
}
