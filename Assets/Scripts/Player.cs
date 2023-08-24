using System;
using System.Collections;
using System.Collections.Generic;
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
    [Tooltip("How many hit points the player has")]
    private int _hitPoints = 100;

    [SerializeField]
    [Tooltip("The maximum number of hit points the player can have")]
    private int _maxHitPoints = 100;

    [SerializeField]
    [Tooltip("How fast the player moves (how exaclty I don't know)")]
    private float _playerMoveRate = 5;
    private float _baseMoveRate;

    private bool _hasPickedUpSkateboard = false;
    private float _timeElapsedSinceLastSkateboard = 0.0f;

    private bool _hasPickedUpUmbrella = false;
    private float _timeElapsedSinceLastUmbrella = 0.0f;

    [SerializeField]
    [Tooltip("How long a time-based pickup should last")]
    private float _timeBasedPickupDuration = 10f;

    [SerializeField]
    [Tooltip("How much damage is reduced for the player")]
    private float _damageReductionAmount = 0.0f;

    private HealthBar _healthBar;
    public Animator animator;
    Vector2 movement;
    bool facingRight = false;

    public AudioSource takeDamageSound;
    public Vector3 lastPosition;

    private Rigidbody2D _rigidBody;
    private IEnumerator coroutine;
    private bool _isDead = false;

    // Start is called before the first frame update

    static Player _instance;

    public static Player Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<Player>();
            }
            return _instance;
        }
    }

    void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
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
        if (_isDead) {
            return;
        }

        lastPosition = transform.position;

        var movementVector = new Vector2(
            Input.GetAxis("Horizontal") *  _playerMoveRate,
            Input.GetAxis("Vertical") * _playerMoveRate
        );
        _rigidBody.velocity = movementVector;

        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x > 0) {
            facingRight = true;
        } else if (movement.x < 0) {
            facingRight = false;
        } // if 0, don't modify
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("FacingRight", facingRight);

        // fire a dart once enough time has elapsed
        Dart.TimeElapsedSinceLastDart += Time.deltaTime;

        if (Raven.IsEnabled)
        {
            Raven.TimeElapsedSinceLastRaven += Time.deltaTime;
        }

        if (Starfish.IsEnabled && !Starfish.IsActive) 
        {
            // don't count while starfish is still orbiting
            Starfish.TimeElapsedSinceLastStarfish += Time.deltaTime;
        }

        if (Dart.TimeElapsedSinceLastDart > Dart.Cooldown)
        {
            Dart.TimeElapsedSinceLastDart = 0.0f;

            float degreesToRotate = Dart.SpreadInDegrees;
            int numberOfDartPairs = Dart.BaseCount / 2;

            // instantiate new dart(s)
            for (int i = 0; i < numberOfDartPairs; i++) 
            {
                for (int j = 0; j < 2; j++) 
                {
                    var dart = Instantiate(_dartPrefab);
                    dart.transform.parent = transform.parent;
                    Vector3 direction = CalculateDartDirection();
                    direction = RotateDirection(direction, degreesToRotate);
                    degreesToRotate *= -1; // to get the proper rotation for the next dart, which is on the other side of the center dart
                    dart.SetDirection(direction);

                    // set the dart's position to the player's position + a little bit
                    // outside the player in the direction of the mouse cursor
                    var distanceOutsidePlayer = 2.0f;
                    dart.transform.position = transform.position + direction.normalized * distanceOutsidePlayer;
                }

                degreesToRotate -= degreesToRotate / numberOfDartPairs;

                // TODO: need to write coroutine to space out darts and remove spread
            }

            if (Dart.BaseCount % 2 != 0) 
            {
                // odd number of darts, so we need to create a center dart
                var dart = Instantiate(_dartPrefab);
                dart.transform.parent = transform.parent;
                Vector3 direction = CalculateDartDirection();
                direction = RotateDirection(direction, 0);
                dart.SetDirection(direction);

                // set the dart's position to the player's position + a little bit
                // outside the player in the direction of the mouse cursor
                var distanceOutsidePlayer = 2.0f;
                dart.transform.position = transform.position + direction.normalized * distanceOutsidePlayer;
            }

            int numberOfAdditionalDartPairs = Dart.AdditionalDarts / 2;

            // instantiate backwards dart(s), if any
            for (int i = 0; i < numberOfAdditionalDartPairs; i++)
            {
                for (int j = 0; j < 2; j++) 
                {
                    var dart = Instantiate(_dartPrefab);
                    dart.transform.parent = transform.parent;
                    Vector3 direction = CalculateDartDirection();
                    direction = RotateDirection(direction, degreesToRotate);
                    degreesToRotate *= -1; // to get the proper rotation for the next dart, which is on the other side of the center dart
                    dart.SetDirection(-direction); // bc these ones go backwards

                    // set the dart's position to the player's position + a little bit
                    // outside the player in the direction of the mouse cursor
                    var distanceOutsidePlayer = 2.0f;
                    dart.transform.position = transform.position + direction.normalized * distanceOutsidePlayer;
                }
                // TODO: need to write coroutine to space out darts; might have to do this within dart
            }

            if (Dart.AdditionalDarts % 2 != 0) 
            {
                // odd number of darts, so we need to create a center dart
                var dart = Instantiate(_dartPrefab);
                dart.transform.parent = transform.parent;
                Vector3 direction = CalculateDartDirection();
                direction = RotateDirection(direction, 0);
                dart.SetDirection(-direction);

                // set the dart's position to the player's position + a little bit
                // outside the player in the direction of the mouse cursor
                var distanceOutsidePlayer = 2.0f;
                dart.transform.position = transform.position + direction.normalized * distanceOutsidePlayer;
            }
        }

        if (Raven.IsEnabled && Raven.TimeElapsedSinceLastRaven > Raven.Cooldown)
        {
            Raven.TimeElapsedSinceLastRaven = 0.0f;
            int numberOfRavens = Raven.BaseCount + Raven.AdditionalRavens;
            for (int i = 0; i < numberOfRavens; i++) 
            {
                var raven = Instantiate(_ravenPrefab); 
                raven.identifier = i;
                raven.transform.parent = transform.parent;
                raven.TargetClosestEnemy();
            }
            Raven.CurrentTargets.Clear(); // reset raven targeting
        }

        if (Starfish.IsEnabled && !Starfish.IsActive && Starfish.TimeElapsedSinceLastStarfish > Starfish.Cooldown)
        {
            Starfish.TimeElapsedSinceLastStarfish = 0.0f;
            int numberOfStarfish = Starfish.BaseCount + Starfish.AdditionalStarfish;
            float degreesBetweenStarfish = 360 / numberOfStarfish;
            for (int i = 0; i < numberOfStarfish; i++)
            {
                _starfishPrefab.DegreesToNextStarfish = degreesBetweenStarfish;
                _starfishPrefab.identifier = i;
                var starfish = Instantiate(_starfishPrefab);
                starfish.transform.parent = transform;
            }
        }

        // remove pickup effects if any are time-based and expiring
        if (_hasPickedUpSkateboard)
        {
            _timeElapsedSinceLastSkateboard += Time.deltaTime;
            if (_timeElapsedSinceLastSkateboard > _timeBasedPickupDuration)
            {
                // reset player move speed back to normal if time is up for skateboard pickup
                _hasPickedUpSkateboard = false;
                _playerMoveRate = _baseMoveRate;
                _timeElapsedSinceLastSkateboard = 0.0f;
            }
        }

        if (_hasPickedUpUmbrella)
        {
            _timeElapsedSinceLastUmbrella += Time.deltaTime;
            if (_timeElapsedSinceLastUmbrella > _timeBasedPickupDuration)
            {
                // reset player damage reduction to 0
                _hasPickedUpUmbrella = false;
                _damageReductionAmount = 0.0f;
                _timeElapsedSinceLastUmbrella = 0.0f;
            }
        }
    }

    private Vector3 CalculateDartDirection() 
    {
        // dart moves in the direction of the current mouse cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;

        return direction;
    }

    private Vector3 RotateDirection(Vector3 direction, float degreesToRotate)
    {
        float degreesInRadians = degreesToRotate * Mathf.Deg2Rad;
        return new Vector3(
            direction.x * Mathf.Cos(degreesInRadians) - direction.y * Mathf.Sin(degreesInRadians),
            direction.x * Mathf.Sin(degreesInRadians) + direction.y * Mathf.Cos(degreesInRadians) 
        );
    }
    
    IEnumerator Wait(float _waitTime) {
        _isDead = true;
        yield return new WaitForSeconds(_waitTime);
        // emit player death event
        EventManager.TriggerEvent("PlayerDeath");
    }
    public void TakeDamage(int damage = 0)
    {
        _hitPoints -= (int) (damage * (1 - _damageReductionAmount));
        _hitPoints = Math.Max(_hitPoints, 0); // don't let the player have negative hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);

        if (_hitPoints == 0)
        {
            animator.SetTrigger("Dead");
            coroutine = Wait(1.2f);
            StartCoroutine(coroutine);
    
        } else {
            // play damage sound effect
            takeDamageSound.Play();
        }
    }

    public void HealDamage(int healAmount = 0) {
        _hitPoints += healAmount;
        _hitPoints = Math.Min(_hitPoints, 100); // don't let the player have more than 100 hit points

        _healthBar.SetHealth(1.0f * _hitPoints / _maxHitPoints);
    }

    public void SpeedUp(int newSpeed, bool hasSkateboard = false)
    {
        _playerMoveRate = newSpeed;
        _hasPickedUpSkateboard = hasSkateboard;

        if (_hasPickedUpSkateboard)
        {
            // need to reset in case player already had an active skateboard
            _timeElapsedSinceLastSkateboard = 0.0f;
        }
    }

    public void ReduceDamage(float reductionPercentage, bool hasUmbrella = false)
    {
        _damageReductionAmount = reductionPercentage;
        _hasPickedUpUmbrella = hasUmbrella;

        if (_hasPickedUpUmbrella)
        {
            _timeElapsedSinceLastUmbrella = 0.0f;
        }
    }
}