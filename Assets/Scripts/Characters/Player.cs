using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The prefab for the player's dart (starting projectile)")]
    private Dart _dartPrefab;

    [SerializeField]
    [Tooltip("The prefab for the player's Raven")]
    private Raven _ravenPrefab;

    [SerializeField]
    [Tooltip("The prefab for the player's Starfish")]
    private Starfish _starfishPrefab;

    [SerializeField]
    [Tooltip("The prefab for the player's Schnitzel")]
    private Schnitzel _schnitzelPrefab;

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

    private Dictionary<Pickup.PickupType, float> _activePlayerEffects =
        new Dictionary<Pickup.PickupType, float>();

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

        Dart.TimeElapsedSinceLastDart = Dart.Cooldown - 1f; // shoot right when the game starts
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

        UpdateDarts();
        UpdateRavens();
        UpdateStarfish();
        UpdateSchnitzel();
        UpdatePickups();
    }

    void UpdateDarts()
    {
        if (Dart.IsShooting)
        {
            // if the dart is already firing, exit early (we only start counting
            // after the dart has CEASED firing)
            return;
        }

        // once enough time has elapsed, fire the darts
        Dart.TimeElapsedSinceLastDart += Time.deltaTime;
        if (Dart.TimeElapsedSinceLastDart > Dart.Cooldown && !Dart.IsShooting)
        {
            StartCoroutine(Dart.ShootDarts(_dartPrefab, gameObject));
        }
    }

    void UpdateRavens()
    {
        if (!Raven.IsEnabled)
        {
            return;
        }

        Raven.TimeElapsedSinceLastRaven += Time.deltaTime;
        if (Raven.TimeElapsedSinceLastRaven > Raven.Cooldown)
        {
            Raven.TimeElapsedSinceLastRaven = 0.0f;

            // note: parent transform is the parent container
            Raven.Fire(_ravenPrefab, transform.parent);
        }
    }

    void UpdateStarfish()
    {
        if (!Starfish.IsEnabled)
        {
            return;
        }

        if (Starfish.IsActive)
        {
            // if starfish is already orbiting nothing to do
            return;
        }

        Starfish.TimeElapsedSinceLastStarfish += Time.deltaTime;
        if (Starfish.TimeElapsedSinceLastStarfish > Starfish.Cooldown)
        {
            Starfish.TimeElapsedSinceLastStarfish = 0.0f;

            // note: parent transform is the player (because starfish orbits)
            //       around the player
            Starfish.Fire(_starfishPrefab, this.transform);
        }
    }

    void UpdateSchnitzel()
    {
        if (Schnitzel.IsShooting)
        {
            return;
        }

        if (!Schnitzel.IsEnabled)
        {
            return;
        }

        Schnitzel.TimeElapsedSinceLastSchnitzel += Time.deltaTime;
        if (Schnitzel.TimeElapsedSinceLastSchnitzel > Schnitzel.Cooldown)
        {
            StartCoroutine(Schnitzel.ShootSchnitzels(_schnitzelPrefab, gameObject));
        }
    }

    void UpdatePickups()
    {
        foreach (KeyValuePair<Pickup.PickupType, float> kv in _activePlayerEffects.ToList())
        {
            if (Time.time < kv.Value)
            {
                continue;
            }

            switch (kv.Key)
            {
                case Pickup.PickupType.Skateboard:
                    _playerMoveRate = _baseMoveRate;
                    _activePlayerEffects.Remove(Pickup.PickupType.Skateboard);
                    EventManager.TriggerEvent("PickupExpired", new EventData("Skateboard"));
                    break;
                case Pickup.PickupType.Umbrella:
                    _damageReductionAmount = 0f;
                    _activePlayerEffects.Remove(Pickup.PickupType.Umbrella);
                    EventManager.TriggerEvent("PickupExpired", new EventData("Umbrella"));
                    break;
                default:
                    throw new Exception("Unknown Pickup Type: " + kv.Key.ToString());
            }
            _activePlayerEffects.Remove(kv.Key);
        }
    }

    IEnumerator Wait(float _waitTime)
    {
        _isDead = true;
        yield return new WaitForSeconds(_waitTime);
        // emit player death event
        EventManager.TriggerEvent("PlayerDeath");
    }

    public void TakeDamage(int damage = 0)
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

    public void HealDamage(int healAmount = 0)
    {
        _hitPoints += healAmount;
        _hitPoints = Math.Min(_hitPoints, 100); // don't let the player have more than 100 hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);

        Vector2 textPosition = new Vector2(transform.position.x, transform.position.y + 1.0f);
        _playerTextPrefab.Spawn(transform.root, textPosition, "+" + healAmount + " health!");
    }

    public void SpeedUp(int newSpeed, float duration = 0)
    {
        _playerMoveRate = newSpeed;
        _activePlayerEffects[Pickup.PickupType.Skateboard] = Time.time + duration;

        Vector2 textPosition = new Vector2(transform.position.x, transform.position.y + 1.0f);
        string speedRate = (newSpeed / _baseMoveRate).ToString();
        _playerTextPrefab.Spawn(transform.root, textPosition, speedRate + "x speed!");
    }

    public void ReduceDamage(float reductionPercentage, float duration = 0f)
    {
        _damageReductionAmount = reductionPercentage;

        _activePlayerEffects[Pickup.PickupType.Umbrella] = Time.time + duration;

        Vector2 textPosition = new Vector2(transform.position.x, transform.position.y + 1.0f);
        string formatPercentage = (reductionPercentage * 100).ToString();
        _playerTextPrefab.Spawn(
            transform.root,
            textPosition,
            formatPercentage + "% dmg reduction!"
        );
    }
}
