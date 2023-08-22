using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The enemy prefab to spawn")]
    private GameObject _enemyPrefab;

    [SerializeField]
    [Tooltip("How frequently enemies spawn (in seconds)")]
    private float _enemySpawnRate = 2.0f;

    [SerializeField]
    [Tooltip("The pickup prefab to spawn")]
    private GameObject _pickupPrefab;

    [SerializeField]
    [Tooltip("How frequently pickups spawn (in seconds)")]
    private float _pickupSpawnRate = 8.0f;

    [SerializeField]
    [Tooltip("The maximum number of pickups allowed on screen at any given moment")]
    private int _maxPickupsOnScreen = 5;

    [SerializeField]
    [Tooltip("The level up UI prefab to spawn")]
    private GameObject _levelUpUIPrefab;

    [SerializeField]
    [Tooltip("Starting XP")]
    private float _xp = 0;

    [SerializeField]
    [Tooltip("The level's tilemap")]
    private Tilemap _floor;

    // the player's accumulated score so far
    private int _score = 0;

    // the score a player must get to for the next weapon upgrade
    private int _nextLevelScoreMilestone;
    private int[] _levelMilestones = {50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600}; // TODO: update
    private int _currentLevel = 0;

    private float _lastEnemySpawnTime = 0.0f;
    private float _lastSpawnRampUp = 0.0f;
    private int _spawnRampUpInterval = 10;

    private int _hpRampUpInterval = 20;
    private float _lastHpRampUp = 0.0f;
    private int _maxHitPoints = 30;
    private float _lastPickupSpawnTime = 0.0f;

    private int _pickupsOnScreen = 0; 

    private enum GameState
    {
        Playing,
        GameOver
    }

    private GameState _gameState;

    private HUD _hud;
    private GameObject _levelContainer;


    // Start is called before the first frame update
    void Start()
    {
        _nextLevelScoreMilestone = _levelMilestones[0];

        _gameState = GameState.Playing;
        _hud = GameObject.Find("HUD").GetComponent<HUD>();
        _hud.SetXp(1.0f * _score);

        _levelContainer = GameObject.Find("Level");

        _lastEnemySpawnTime = Time.time;
        _lastPickupSpawnTime = Time.time;
        _pickupsOnScreen = 1;

        SetCurrentLevel(_currentLevel);

        EventManager.AddListener("EnemyDestroyed", (eventData) => {
            OnEnemyDestroyed((int)eventData.Data);
        });
        EventManager.AddListener("PickupGrabbed", (eventData) => {
            OnPickupGrabbed((int)eventData.Data);
        });
        EventManager.AddListener("PlayerDeath", (eventData) => {
            OnPlayerDeath();
        });
    }

    private void OnPickupGrabbed(int scoreValue) {
        SetScore(_score + scoreValue);
        _pickupsOnScreen -= 1;

        Debug.Log("GameManager.OnPickupGrabbed: Score is now " + _score);
    }

    private void OnEnemyDestroyed(int scoreValue)
    {
        SetScore(_score + scoreValue);
        Debug.Log("GameManager.OnEnemyDestroyed: Score is now " + _score);
        Debug.Log("GameManager.OnEnemyDestroyed: Next Milestone at score " + _nextLevelScoreMilestone);
    
    }
     
    private void OnPlayerDeath() {
        _gameState = GameState.GameOver;

        // STOP THE GAME
        // -- by setting the timescale to 0, we stop all time-based operations
        Time.timeScale = 0;

        _hud.ShowGameOver();
    }

    private void SetScore(int score) {
        _score = score;
        _xp = 1.0f * _score / _nextLevelScoreMilestone;

        _hud.SetScore(_score);
        _hud.SetXp(_xp);

        Debug.Log("GameManager.SetScore: Score is now " + _score);   
        Debug.Log("GameManager.SetScore: Xp is now " + _xp);        

    }

    private void SetCurrentLevel(int level) {
        _currentLevel = level;

        _hud.SetCurrentLevel(_currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        // every 2 seconds, instantiate a new enemy prefab and position at the edge
        // of the viewport;
        if (Time.time - _lastEnemySpawnTime > _enemySpawnRate)
        {
            _lastEnemySpawnTime = Time.time;

            SpawnEnemy();
        }

        if (Time.time - _lastSpawnRampUp > _spawnRampUpInterval) {
            _enemySpawnRate -= 0.05f;
            _lastSpawnRampUp = Time.time;
        }

        if (Time.time - _lastHpRampUp > _hpRampUpInterval) {
            Debug.Log("HP ramped up to " + _maxHitPoints);
            _maxHitPoints += 10;
            _lastHpRampUp = Time.time;
        }
        if (Time.time - _lastPickupSpawnTime > _pickupSpawnRate)
        {
            _lastPickupSpawnTime = Time.time;
            
            SpawnPickup();
        }

        if (_score >= _nextLevelScoreMilestone && _currentLevel < _levelMilestones.Length) 
        {
            _currentLevel++;
            SetCurrentLevel(_currentLevel);
            
            // reset xp bar to 0 after leveling up 
            _hud.SetXp(0);

            // we don't want to set the next milestone if we just reached the final level (there 
            // are no more milestones)
            if (_currentLevel < _levelMilestones.Length) 
            {
                _nextLevelScoreMilestone = _levelMilestones[_currentLevel];
            }
            GameObject levelUpUI = Instantiate(_levelUpUIPrefab);
        }

        
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(_enemyPrefab as GameObject);
        enemy.GetComponent<Enemy>().hitpoints = _maxHitPoints;
        enemy.transform.parent = _levelContainer.transform;

        // viewport coords:
        //   (0, 0) is bottom left
        //   (1, 1) is top right
        var spawnViewportCoord = new Vector3(0.0f, 0.0f, 0.0f);

        // randomly choose a side to spawn on, then randomly choose a point on that side
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // top
                spawnViewportCoord = new Vector3(Random.Range(0.0f, 1.0f), 1.1f, 0.0f);
                break;
            case 1: // right
                spawnViewportCoord = new Vector3(1.1f, Random.Range(0.0f, 1.0f), 0.0f);
                break;
            case 2: // bottom
                spawnViewportCoord = new Vector3(Random.Range(0.0f, 1.0f), -0.1f, 0.0f);
                break;
            case 3: // left
                spawnViewportCoord = new Vector3(-0.1f, Random.Range(0.0f, 1.0f), 0.0f);
                break;
        }
        enemy.transform.position = Camera.main.ViewportToWorldPoint(spawnViewportCoord);
    }

    private void SpawnPickup() 
    {
        if (_pickupsOnScreen >= _maxPickupsOnScreen)
        {
            return;
        }

        GameObject pickup = Instantiate(_pickupPrefab as GameObject);
        pickup.transform.parent = _levelContainer.transform;

        float xMin = _floor.cellBounds.xMin;
        float xMax = _floor.cellBounds.xMax;
        float yMin = _floor.cellBounds.yMin;
        float yMax = _floor.cellBounds.yMax;

        Vector3 spawnViewportCoord = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0.0f);
        
        pickup.transform.position = spawnViewportCoord; //Camera.main.ViewportToWorldPoint(spawnViewportCoord);
        _pickupsOnScreen++;
    }

}
