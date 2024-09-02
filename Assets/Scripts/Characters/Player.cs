using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public WeaponManager WeaponManager;

    [SerializeField]
    [Tooltip("The prefab for the player's Schnitzel")]
    private SchnitzelProjectile _schnitzelPrefab;

    [SerializeField]
    [Tooltip("How many hit points the player has")]
    private int _hitPoints = 100;

    [SerializeField]
    [Tooltip("The maximum number of hit points the player can have")]
    private int _maxHitPoints = 100;

    [SerializeField]
    [Tooltip("How fast the player moves (how exaclty I don't know)")]
    private float _playerMoveRate = 2.5f;

    private float _baseMoveRate;

    [SerializeField]
    [Tooltip("The prefab to use for the player text")]
    private PlayerText _playerTextPrefab;

    [SerializeField]
    [Tooltip("How much damage is reduced for the player")]
    private float _damageReductionAmount = 0.0f;

    public enum PlayerEffectTypes
    {
        SpeedUp,
        DamageResist,
    }

    private HealthBar _healthBar;
    public Animator animator;
    Vector2 movement;
    bool facingRight = false;

    public AudioSource takeDamageSound;
    public Vector3 lastPosition;

    private Rigidbody2D _rigidBody;
    private IEnumerator coroutine;
    private bool _isDead = false;

    private BattleSceneManager _gameManager;

    static Player _instance;

    private float _restoreSpeedTime = 0.0f;
    private float _restoreDamageResistTime = 0.0f;

    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Player>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        _gameManager = GameObject.Find("GameManager").GetComponent<BattleSceneManager>();
    }

    void Start()
    {
        _baseMoveRate = _playerMoveRate;

        // get a reference to the Healthbar component
        _healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        takeDamageSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead)
        {
            return;
        }

        // get game manager
        if (!_gameManager.IsPlaying)
        {
            return;
        }

        lastPosition = transform.position;

        var movementVector = new Vector2(
            Input.GetAxis("Horizontal") * _playerMoveRate,
            Input.GetAxis("Vertical") * _playerMoveRate
        );
        _rigidBody.velocity = movementVector;

        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x > 0)
        {
            facingRight = true;
        }
        else if (movement.x < 0)
        {
            facingRight = false;
        } // if 0, don't modify
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("FacingRight", facingRight);
    }

    IEnumerator Wait(float _waitTime)
    {
        _isDead = true;
        yield return new WaitForSeconds(_waitTime);
        // emit player death event
        EventManager.TriggerEvent("PlayerDeath");
    }

    public void ApplyDamage(int damage = 0)
    {
        _hitPoints -= (int)(damage * (1 - _damageReductionAmount));
        _hitPoints = Math.Max(_hitPoints, 0); // don't let the player have negative hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);

        if (_hitPoints == 0)
        {
            animator.SetTrigger("Dead");
            coroutine = Wait(1.2f);
            StartCoroutine(coroutine);
        }
        else
        {
            // play damage sound effect
            takeDamageSound.Play();
        }
    }

    public void ApplyHeal(int healAmount = 0)
    {
        _hitPoints += healAmount;
        _hitPoints = Math.Min(_hitPoints, 100); // don't let the player have more than 100 hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);
    }

    public void ApplySpeedUp(int speedMultiple, float duration = 0f)
    {
        _playerMoveRate = _baseMoveRate * speedMultiple;
        if (duration > 0)
        {
            _restoreSpeedTime = Time.time + duration;
            StartCoroutine(RestoreSpeed(duration));
        }
    }

    public IEnumerator RestoreSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        // note: the time at which to restore the speed could have been pushed out
        if (Time.time > _restoreSpeedTime)
        {
            _playerMoveRate = _baseMoveRate;
        }
    }

    public void ApplyDamageResist(float reductionPercentage, float duration = 0f)
    {
        _damageReductionAmount = reductionPercentage;
        if (duration > 0)
        {
            _restoreDamageResistTime = Time.time + duration;
            StartCoroutine(RestoreDamageResist(duration));
        }
    }

    public IEnumerator RestoreDamageResist(float duration)
    {
        yield return new WaitForSeconds(duration);
        // note: the time at which to restore the damage resist could have been pushed out
        if (Time.time > _restoreDamageResistTime)
        {
            _damageReductionAmount = 0f;
        }
    }

    public void SpawnPlayerText(string text)
    {
        Vector2 textPosition = new Vector2(transform.position.x, transform.position.y + 1.0f);
        _playerTextPrefab.Spawn(transform.root, textPosition, text);
    }
}
