using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BattleSceneManager : MonoBehaviour
{
    [Header("Game Properties")]
    [SerializeField]
    [Tooltip("How frequently enemies spawn (in seconds)")]
    private float _enemySpawnRate = 2.0f;

    [SerializeField]
    [Tooltip("The fastest possible spawn rate for enemies (in seconds)")]
    private float _enemySpawnRateFloor = 0.5f;

    [SerializeField]
    [Tooltip("How frequently waves of linear heads spawn (in seconds)")]
    private float _linearHeadSpawnRate = 30.0f;

    [SerializeField]
    [Tooltip("The slowest possible spawn rate for waves of linear heads (in seconds)")]
    private float _linearHeadSpawnRateFloor = 8.0f; // 5s

    [SerializeField]
    [Tooltip("What level linear head waves start spawning")]
    private int _linearHeadSpawnLevelFloor = 3;

    [SerializeField]
    [Tooltip("How frequently pickups spawn (in seconds)")]
    private float _pickupSpawnRate = 15.0f;

    [SerializeField]
    [Tooltip("The maximum number of pickups allowed on screen at any given moment")]
    private int _maxPickupsOnScreen = 7;

    [SerializeField]
    [Tooltip("The current level")]
    private int _currentLevel = 0;

    [SerializeField]
    [Tooltip("Maximum wave size scale factor")]
    private float _maxWaveSizeScaleFactor = 0.67f;

    [SerializeField]
    [Tooltip("Time when death appears (in seconds)")]
    private float _deathAppearanceTime = 600; // 10 minutes

    [SerializeField]
    [Tooltip("The plane to spawn enemies on")]
    private GameObject _spawnPlane;

    [SerializeField]
    [Tooltip("Starting XP")]
    private float _xp = 0;

    [Header("Prefabs")]
    [SerializeField]
    [Tooltip("The sentaur enemy prefab to spawn")]
    private GameObject _sentaurEnemyPrefab;

    [SerializeField]
    [Tooltip("The ant enemy prefab to spawn")]
    private GameObject _antEnemyPrefab;

    [SerializeField]
    [Tooltip("The head enemy prefab to spawn")]
    private GameObject _headEnemyPrefab;

    [SerializeField]
    [Tooltip("The mantis enemy prefab to spawn")]
    private GameObject _mantisEnemyPrefab;

    [SerializeField]
    [Tooltip("The death enemy prefab to spawn")]
    private GameObject _deathEnemyPrefab;

    [SerializeField]
    [Tooltip("The linear enemy prefab to spawn")]
    private GameObject _linearEnemyPrefab;

    [SerializeField]
    [Tooltip("List of pickup prefabs to randomly spawn")]
    private GameObject[] _pickupPrefabs;

    [SerializeField]
    [Tooltip("The level up UI prefab to spawn")]
    private GameObject _levelUpUI;

    [SerializeField]
    [Tooltip("The parent UI element containing the active pickups")]
    private GameObject _activePickupContainer;

    [SerializeField]
    [Tooltip("The parent UI element containing the active upgrades")]
    public GameObject _activeUpgradesContainer;

    [SerializeField]
    [Tooltip("Background Music")]
    private AudioSource _backgroundMusic;

    // the player's accumulated score so far
    private int _score = 0;

    public int GetScore() => _score;

    // the score a player must get to for the next weapon upgrade
    private int _nextLevelScoreMilestone;

    [SerializeField]
    private int[] _levelMilestones =
    {
        50, // level 2
        150,
        300,
        550,
        900,
        1400,
        2050,
        2900,
        3950, // level 10
        5250,
        6850,
        8800,
        11150,
        13950,
        17250,
        21150,
        25800,
        31350,
        38000, // level 20
        46000,
        55600, // level 22 (max)
    }; // for testing: {50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900};
    private int _nextLevelXpMilestone;

    // trackinf the previous level score milestone for xp bar
    private int _prevLevelXpMilestone;
    private int _previousLevel = 0;

    private float _lastEnemySpawnTime = 0.0f;

    private float _lastWaveSpawnTime = 0.0f;

    private float _lastSpawnRampUp = 0.0f;
    private int _spawnRampUpInterval = 10;

    public enum EnemyType
    {
        Sentaur = 0,
        Ant = 1,
        DiagonalHead = 2,
        Mantis = 3,
        LinearHead = 4,
    };

    // what level enemies start appearing
    private Dictionary<EnemyType, int> _levelEnemyGate = new Dictionary<EnemyType, int>
    {
        { EnemyType.Sentaur, 0 }, // start
        { EnemyType.Ant, 2 }, // level 3
        { EnemyType.DiagonalHead, 4 }, // level 5
        { EnemyType.Mantis, 6 }, // level 7
    };

    [SerializeField]
    [Tooltip("How frequently the max hitpoints of enemies ramp up (in seconds)")]
    private int _hpRampUpInterval = 60;

    [SerializeField]
    [Tooltip("How much to increase the max hitpoints of enemies by each interval")]
    private int _hpRampUpValue = 10;

    private float _lastHpRampUp = 0.0f;

    // By how much each enemy's hit points is modified when spawned
    private int _enemyHitPointModifier = 0;

    private float _lastPickupSpawnTime = 0.0f;

    private int _pickupsOnScreen = 0;

    private enum GameState
    {
        Playing,
        GameOver,
        Paused
    }

    public bool IsPlaying => _gameState == GameState.Playing;

    private GameState _gameState;

    private HUD _hud;
    private GameObject _levelContainer;

    private float _gameStartTime;
    private bool _isDeathEnemyPresent = false;

    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        _nextLevelXpMilestone = _levelMilestones[0];

        _gameState = GameState.Playing;
        Time.timeScale = 1; // in case time scale was set to 0 (e.g. on death)

        _hud = GameObject.Find("HUD").GetComponent<HUD>();
        _hud.SetXp(1.0f * _score);

        _levelContainer = GameObject.Find("Level");
        _backgroundMusic = GameObject.Find("Level").GetComponent<AudioSource>();

        _lastEnemySpawnTime = Time.time;
        _lastWaveSpawnTime = Time.time;
        _lastPickupSpawnTime = Time.time;
        _pickupsOnScreen = 1;

        _gameStartTime = Time.time;

        SetCurrentLevel(_currentLevel);

        EventManager.AddListener(
            "EnemyDestroyed",
            (eventData) =>
            {
                OnEnemyDestroyed((int)eventData.Data);
            }
        );
        EventManager.AddListener(
            "PickupGrabbed",
            (eventData) =>
            {
                OnPickupGrabbed((List<object>)eventData.Data);
            }
        );
        EventManager.AddListener(
            "PickupExpired",
            (eventData) =>
            {
                OnPickupExpired((string)eventData.Data);
            }
        );
        EventManager.AddListener(
            "PlayerDeath",
            (eventData) =>
            {
                OnPlayerDeath();
            }
        );
        EventManager.AddListener(
            "XpEarned",
            (eventData) =>
            {
                OnXpEarned((int)eventData.Data);
            }
        );

        EventManager.AddListener(
            "TryAgain",
            (eventData) =>
            {
                OnTryAgain();
            }
        );

        EventManager.AddListener(
            "Quit",
            (eventData) =>
            {
                OnQuit();
            }
        );

        EventManager.AddListener(
            "UpgradeChosen",
            (eventData) =>
            {
                OnUpgradeChosen((UpgradeEventData)eventData.Data);
            }
        );
    }

    private void OnPickupGrabbed(List<object> eventData)
    {
        int scoreValue = (int)eventData[0];
        SetScore(_score + scoreValue);
        _pickupsOnScreen -= 1;

        string pickupName = (string)eventData[1];
        float effectDuration = (float)eventData[2];

        if (pickupName == "Mozart")
        {
            StartCoroutine(SetAllXpDropsToMoving());
        }
        if (effectDuration > 0)
        {
            _activePickupContainer.transform.Find(pickupName).gameObject.SetActive(true);
        }
    }

    // empty co-routine
    private IEnumerator SetAllXpDropsToMoving()
    {
        // get all xp drops and set moving = true
        var xpDrops = GameObject.FindGameObjectsWithTag("XpDrop");
        Debug.Log("Got x xp drops: " + xpDrops.Length);
        foreach (var xpDrop in xpDrops)
        {
            xpDrop.GetComponent<XpDrop>().SetMoving();
        }
        yield return null;
    }

    private void OnPickupExpired(string pickupName)
    {
        _activePickupContainer.transform.Find(pickupName).gameObject.SetActive(false);
    }

    private void OnXpEarned(int xp)
    {
        _xp += xp;
        UpdateXpProgress();

        // xp picked up also adds to score
        // this is important late game -- even if xp doesn't help you level up,
        // at least it adds to score
        AddScore(xp);
    }

    private void OnQuit()
    {
        Application.Quit();
    }

    private void OnTryAgain()
    {
        // reload this scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            "BattleScene",
            UnityEngine.SceneManagement.LoadSceneMode.Single
        );
    }

    private void UpdateXpProgress()
    {
        float xpProgress;
        if (_currentLevel == 0)
        {
            xpProgress = 1.0f * _xp / _nextLevelXpMilestone;
        }
        else
        {
            xpProgress =
                1.0f
                * (_xp - _prevLevelXpMilestone)
                / (_nextLevelXpMilestone - _prevLevelXpMilestone);
        }
        _hud.SetXp(xpProgress);
    }

    private void OnEnemyDestroyed(int scoreValue)
    {
        SetScore(_score + scoreValue);
    }

    public void PauseGame()
    {
        _gameState = GameState.Paused;

        // stop playing the background music when the game stops
        _backgroundMusic.Stop();

        // STOP THE GAME
        // -- by setting the timescale to 0, we stop all time-based operations
        Time.timeScale = 0;

        _hud.ShowPause();
    }

    public void UnpauseGame()
    {
        _gameState = GameState.Playing;

        // resume playing the background music when the game resumes
        _backgroundMusic.Play();

        // RESUME THE GAME
        // -- by setting the timescale to 1, we resume all time-based operations
        Time.timeScale = 1;

        _hud.HidePause();
    }

    private void OnPlayerDeath()
    {
        _gameState = GameState.GameOver;
        // stop playing the background music when the game stops
        _backgroundMusic.Stop();

        // STOP THE GAME
        // -- by setting the timescale to 0, we stop all time-based operations
        Time.timeScale = 0;

        _hud.ShowGameOver();
    }

    private void SetScore(int score)
    {
        _score = score;
        _hud.SetScore(_score);
    }

    private void AddScore(int score)
    {
        SetScore(_score + score);
    }

    private void SetCurrentLevel(int level)
    {
        _currentLevel = level;

        _hud.SetCurrentLevel(_currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameState == GameState.Playing)
                PauseGame();
            else if (_gameState == GameState.Paused)
                UnpauseGame();
        }

        if (_gameState != GameState.Playing)
        {
            return;
        }

        if (!_isDeathEnemyPresent && (Time.time - _gameStartTime > _deathAppearanceTime))
        {
            _isDeathEnemyPresent = true;
            SpawnDeath();
        }

        // every 2 seconds, instantiate a new enemy prefab and position at the edge
        // of the viewport;
        if (Time.time - _lastEnemySpawnTime > _enemySpawnRate)
        {
            _lastEnemySpawnTime = Time.time;

            Spawn();
        }

        if (
            Time.time - _lastWaveSpawnTime > _linearHeadSpawnRate
            && _currentLevel >= _linearHeadSpawnLevelFloor
        )
        {
            _lastWaveSpawnTime = Time.time;

            SpawnWave((_currentLevel + 1) / 2);
        }

        // ramp up spawn rate
        if (Time.time - _lastSpawnRampUp > _spawnRampUpInterval)
        {
            _enemySpawnRate -= 0.05f;
            _enemySpawnRate = Mathf.Max(_enemySpawnRate, _enemySpawnRateFloor);

            _linearHeadSpawnRate -= 1.5f;
            _linearHeadSpawnRate = Mathf.Max(_linearHeadSpawnRate, _linearHeadSpawnRateFloor);

            _lastSpawnRampUp = Time.time;
        }

        // ramp up enemy hp
        if (Time.time - _lastHpRampUp > _hpRampUpInterval)
        {
            _enemyHitPointModifier += _hpRampUpValue;

            Debug.Log(
                "Enemy HP modifier is now "
                    + _enemyHitPointModifier
                    + " ("
                    + (int)(Time.time - _lastHpRampUp)
                    + "s elapsed )"
            );
            _lastHpRampUp = Time.time;
        }
        if (
            _pickupsOnScreen < _maxPickupsOnScreen
            && Time.time - _lastPickupSpawnTime > _pickupSpawnRate
        )
        {
            _lastPickupSpawnTime = Time.time;

            SpawnPickup();
        }

        if (_xp >= _nextLevelXpMilestone && _currentLevel < _levelMilestones.Length)
        {
            _currentLevel++;
            Debug.Log("GameManager.Update: Level Up!");

            _previousLevel = _currentLevel - 1;

            SetCurrentLevel(_currentLevel);

            // reset xp bar to 0 after leveling up
            _hud.SetXp(0);

            // we don't want to set the next milestone if we just reached the final level (there
            // are no more milestones)
            if (_currentLevel < _levelMilestones.Length)
            {
                _nextLevelXpMilestone = _levelMilestones[_currentLevel];
                _prevLevelXpMilestone = _levelMilestones[_previousLevel];
            }
            _levelUpUI.SetActive(true);
        }
    }

    /**
      * Returns the bounding box of the camera viewport in world space coordinates
      */
    private Bounds GetViewportBounds()
    {
        // get the bounding box of the camera viewport in world space coordinates
        // camera viewport has the following coordinate system:
        //   (0, 0) is bottom left
        //   (1, 1) is top right
        var viewportCenterCoords = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        viewportCenterCoords.z = 0; // reset z to 0 so on same plane as enemies

        var viewportBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0.0f));
        var viewportTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f));
        var viewportExtents = new Vector3(
            viewportTopRight.x - viewportBottomLeft.x,
            viewportTopRight.y - viewportBottomLeft.y,
            0f
        );

        // convert to bounds
        return new Bounds(viewportCenterCoords, viewportExtents);
    }

    /**
     * Returns a random position within the spane area
     */
    private Vector3 GetRandomSpawnPoint()
    {
        var rectTransform = _spawnPlane.GetComponent<RectTransform>();
        // get bounds of rect transform
        var rectTransformWorldBounds = new Bounds(rectTransform.position, rectTransform.rect.size);

        var floorSpawnCoord = new Vector3(
            Random.Range(rectTransformWorldBounds.min.x, rectTransformWorldBounds.max.x),
            Random.Range(rectTransformWorldBounds.min.y, rectTransformWorldBounds.max.y),
            0.0f
        );

        return floorSpawnCoord;
    }

    /**
     * Returns a random spawnable position outside of the viewport
     */
    private Vector3 GetRandomSpawnPointOutsideViewport()
    {
        // convert to bounds
        var viewportBounds = GetViewportBounds();

        // randomly choose a coordinate until one is found that is outside the viewport
        Vector3 floorSpawnCoord;
        do
        {
            // choose a random coordinate within rectTransformWorldBounds
            floorSpawnCoord = GetRandomSpawnPoint();

            // loop until a coordinate is chosen outside viewport
        } while (viewportBounds.Contains(floorSpawnCoord));

        return floorSpawnCoord;
    }

    /**
     * Spawns random enemies depending on the level
     */
    private void Spawn()
    {
        GameObject prefab;

        int spawnRange = (int)EnemyType.Sentaur;

        // determine what range of enemies to spawn based on level
        if (_currentLevel >= _levelEnemyGate[EnemyType.Ant])
        {
            spawnRange = (int)EnemyType.Ant;
        }
        if (_currentLevel >= _levelEnemyGate[EnemyType.DiagonalHead])
        {
            spawnRange = (int)EnemyType.DiagonalHead;
        }
        if (_currentLevel >= _levelEnemyGate[EnemyType.Mantis])
        {
            spawnRange = (int)EnemyType.Mantis;
        }

        int spawnChoice = Random.Range((int)EnemyType.Sentaur, spawnRange + 1);

        switch (spawnChoice)
        {
            case (int)EnemyType.Sentaur:
                prefab = _sentaurEnemyPrefab;
                break;
            case (int)EnemyType.Ant:
                prefab = _antEnemyPrefab;
                break;
            case (int)EnemyType.DiagonalHead:
                prefab = _headEnemyPrefab;
                break;
            case (int)EnemyType.Mantis:
                prefab = _mantisEnemyPrefab;
                break;
            default:
                // throw exception; should never get here
                throw new System.Exception("GameManager.Spawn: Invalid spawn choice");
        }

        // number of enemies spawned is random based on level * _maxWaveSizeScaleFactor
        // with default maxWaveScaleFactor of 0.67:
        //   level 1-2: 1 enemy
        //   level 3-4: 1-2 enemies
        //   level 5:   1-3 enemies
        //   level 6-7: 1-4 enemies
        //   level 8:   1-5 enemies ... etc
        int waveSize = Random.Range(1, (int)(_currentLevel * _maxWaveSizeScaleFactor));

        if (spawnChoice == (int)EnemyType.DiagonalHead)
        {
            // halve the number of heads
            waveSize = Math.Max(1, waveSize / 2);
        }
        SpawnEnemies(prefab, waveSize);
    }

    private GameObject InstantiateEnemy(GameObject gameObject)
    {
        GameObject enemy = Instantiate(gameObject);
        enemy.GetComponent<Enemy>().hitpoints += _enemyHitPointModifier;
        enemy.transform.parent = _levelContainer.transform;
        return enemy;
    }

    private void SpawnWave(int count)
    {
        LinearEnemy.Direction direction = (LinearEnemy.Direction)Random.Range(0, 4);

        Vector3 initialOffset = Vector3.zero;
        Vector3 betweenOffset = Vector3.zero;

        switch (direction)
        {
            case LinearEnemy.Direction.Up:
                // then location is player position - 5 units in the y direction
                initialOffset = new Vector3(-count / 2f, -10, 0);
                betweenOffset = new Vector3(1, 0, 0);
                break;
            case LinearEnemy.Direction.Down:
                // then location is player position + 5 units in the y direction
                initialOffset = new Vector3(count / 2f, 10, 0);
                betweenOffset = new Vector3(-1, 0, 0);
                break;
            case LinearEnemy.Direction.Left:
                // then location is player position + 5 units in the x direction
                initialOffset = new Vector3(10, -count / 2f, 0);
                betweenOffset = new Vector3(0, 1, 0);
                break;
            case LinearEnemy.Direction.Right:
                // then location is player position - 5 units in the x direction
                initialOffset = new Vector3(-10, count / 2f, 0);
                betweenOffset = new Vector3(0, -1, 0);
                break;
        }

        Vector3 spawnPosition;

        int spawnCount = 0;
        while (spawnCount < count)
        {
            GameObject enemy = InstantiateEnemy(_linearEnemyPrefab);

            Vector3 initialPosition = Player.Instance.transform.position + initialOffset;

            // from initial position, fan out enemies
            spawnPosition = initialPosition + spawnCount * betweenOffset;

            enemy.transform.position = spawnPosition;
            enemy.GetComponent<LinearEnemy>().SetDirection(direction);

            spawnCount++;
        }
    }

    private void SpawnEnemies(GameObject prefab, int count)
    {
        Vector3 initialPosition = GetRandomSpawnPointOutsideViewport();

        Vector3 spawnPosition;

        // distance between enemies in X direction
        float distanceOffsetX = 0.75f;
        // range of possible y coordinates to spawn enemies at
        float maxRangeY = 2.0f;
        int flipX = 1;

        int spawnCount = 0;
        while (spawnCount < count)
        {
            GameObject enemy = InstantiateEnemy(prefab);

            // from initial position, fan out enemies to the left and right
            spawnPosition =
                initialPosition
                + new Vector3(
                    distanceOffsetX * ((1 + spawnCount) / 2) * flipX,
                    // randomize y within (2, 2)
                    Random.Range(0, maxRangeY),
                    0
                );
            enemy.transform.position = spawnPosition;

            spawnCount++;
            flipX = -flipX;
        }
    }

    private void SpawnDeath()
    {
        GameObject death = Instantiate(_deathEnemyPrefab);
        death.transform.parent = _levelContainer.transform;
        death.transform.position = GetRandomSpawnPointOutsideViewport();
        _gameStartTime = Time.time;
    }

    private void SpawnPickup()
    {
        if (_pickupsOnScreen >= _maxPickupsOnScreen)
        {
            return;
        }

        int index = random.Next(_pickupPrefabs.Length);

        GameObject pickup = Instantiate(_pickupPrefabs[index]);
        pickup.transform.parent = _levelContainer.transform;

        pickup.transform.position = GetRandomSpawnPoint();
        _pickupsOnScreen++;
    }

    private void OnUpgradeChosen(UpgradeEventData upgradeEvent)
    {
        var newLevel = upgradeEvent.Level;
        switch (upgradeEvent.UpgradeType)
        {
            case UpgradeType.CountUp:
                ProjectileBase.UpgradeCount(newLevel);
                // add upgrade to the active upgrades container in the hud
                // i think we need an EventListener like we do in Pickup.cs:84, maybe?
                // _activePickupContainer.transform.Find("Count").gameObject.SetActive(true);
                break;
            case UpgradeType.CooldownDown:
                ProjectileBase.UpgradeCooldown(newLevel);
                break;
            case UpgradeType.DamageUp:
                ProjectileBase.UpgradeDamage(newLevel);
                break;
            case UpgradeType.Dart:
                Dart.UpgradeDart(newLevel);
                break;
            case UpgradeType.Raven:
                Raven.UpgradeRaven(newLevel);
                break;
            case UpgradeType.Starfish:
                Starfish.UpgradeStarfish(newLevel);
                break;
            case UpgradeType.Schnitzel:
                Schnitzel.UpgradeSchnitzel(newLevel);
                break;
            default:
                break;
        }
    }
}
