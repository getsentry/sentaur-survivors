using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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

    [SerializeField]
    [Tooltip("The material to use when the enemy is flashing")]
    private Material _flashMaterial;

    [SerializeField]
    [Tooltip("How long the enemy flashes for when they take damage")]
    private float _flashDuration = 0.2f;

    [SerializeField]
    [Tooltip("How long the enemy's death animation lasts")]
    private float _deathAnimDuration = 0.5f;

    private Material _originalMaterial;
    private Coroutine _flashCoroutine;

    private SpriteRenderer _spriteRenderer;

    protected Rigidbody2D _rigidbody2D;

    private bool _isDead = false;
    protected bool IsDead => _isDead;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _originalMaterial = _spriteRenderer.material;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // move towards the player character
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            var direction = DetermineDirection(player);
            _rigidbody2D.linearVelocity = direction * _speed;

            // flip sprite in x direction if moving left
            _spriteRenderer.flipX = direction.x < 0;
        }
    }

    /**
     * Returns a normalized vector in the direction of the desired movement
     */
    virtual protected Vector2 DetermineDirection(GameObject player)
    {
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

    // material flash trick from: https://www.youtube.com/watch?v=9rZkiEyS66I
    public void Flash()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }
        _flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        _spriteRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(_flashDuration);
        _spriteRenderer.material = _originalMaterial;
    }

    public void Knockback(Vector2 direction, float force)
    {
        _rigidbody2D.AddForce(direction * force);
    }

    public virtual void Death(bool leaveXp = false)
    {
        _isDead = true;

        // disable all colliders and hitboxes
        // -- that way enemy can't get hit again while they're dying/shrinking
        DisableHitboxes();

        // shrink (scale to 1) before being destroyed
        transform
            .DOScale(0.01f, _deathAnimDuration)
            .OnComplete(() =>
            {
                if (leaveXp)
                {
                    // instantiate an xp drop at this position
                    var xpDrop = Instantiate(
                        _xpDropPrefab,
                        transform.position,
                        Quaternion.identity
                    );
                    xpDrop.SetXp(_xpValue);
                }
                Destroy(gameObject);
            });
    }

    protected void DisableHitboxes()
    {
        var collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // get hitbox and disable
        var hitbox = GetComponentInChildren<Hitbox>();
        hitbox.Disable();
    }

    // Deal damage to the player because they touched
    private void DamagePlayer(Player player)
    {
        Debug.Log("Enemy.DamagePlayer: Player was damaged by " + gameObject.name);

        player.ApplyDamage(_collisionDamage);
    }

    public void TakeDamage(int damage)
    {
        Flash();

        hitpoints -= damage;
        hitpoints = Mathf.Max(hitpoints, 0); // don't let the enemy have negative hit points

        // spawn the damage text above the enemy by 1 unit (32px)
        var damageTextPosition = new Vector2(transform.position.x, transform.position.y + 1.0f);
        _damageTextPrefab.Spawn(transform.root, damageTextPosition, damage);

        if (hitpoints == 0)
        {
            Death(leaveXp: true);

            EventManager.TriggerEvent("EnemyDestroyed", new EventData(_scoreValue));
        }
    }
}
