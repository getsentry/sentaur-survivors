using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The enemy prefab to spawn")]
    private GameObject _enemyPrefab;

    [SerializeField]
    [Tooltip("The diagonal enemy prefab to spawn")]
    private DiagonalEnemy _diagonalEnemyPrefab;

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
    private Tilemap _floor;

    // the player's accumulated score so far
    private int _score = 0;

    // the score a player must get to for the next weapon upgrade
    private int _nextLevelScoreMilestone;
    private int[] _levelMilestones = {50, 150, 350, 650, 1050, 1550, 2150, 2850, 3650, 4550, 5550}; // TODO: update
    // trackinf the previous level score milestone for xp bar
    private int _previousLevelScoreMilestone;
    private int _currentLevel = 0;
    private int _previousLevel = 0;

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

        EventManager.AddListener("EnemyDestroyed", (eventData) =>
        {
            OnEnemyDestroyed((int)eventData.Data);
        });
        EventManager.AddListener("PickupGrabbed", (eventData) =>
        {
            OnPickupGrabbed((int)eventData.Data);
        });
        EventManager.AddListener("PlayerDeath", (eventData) =>
        {
            OnPlayerDeath();
        });
    }

    private void OnPickupGrabbed(int scoreValue)
    {
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

    private void OnPlayerDeath()
    {   
        _gameState = GameState.GameOver;

        // STOP THE GAME
        // -- by setting the timescale to 0, we stop all time-based operations
        Time.timeScale = 0;

        _hud.ShowGameOver();
    }

    private void SetScore(int score)
    {
        _score = score;

        if (_currentLevel == 0) 
        {
            _xp = 1.0f * _score / _nextLevelScoreMilestone;
        } else {
            _xp = 1.0f * (_score - _previousLevelScoreMilestone) / (_nextLevelScoreMilestone - _previousLevelScoreMilestone);
        }

        _hud.SetScore(_score);
        _hud.SetXp(_xp);

        Debug.Log("GameManager.SetScore: Score is now " + _score);   
        Debug.Log("GameManager.SetScore: Milestone level is now " + _nextLevelScoreMilestone);
        Debug.Log("GameManager.SetScore: Xp is now " + _xp);        

    }

    private void SetCurrentLevel(int level)
    {
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

            GameObject prefab;

            // spawn a diagonal enemy 20% of the time
            int random = Random.Range(0, 4);
            if (random == 0) {
                prefab = _diagonalEnemyPrefab.gameObject;
            } else {
                prefab = _enemyPrefab;
            }

            SpawnEnemy(prefab);
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
            Debug.Log("GameManager.Update: level up!");

            _previousLevel = _currentLevel - 1;

            SetCurrentLevel(_currentLevel);
            
            // reset xp bar to 0 after leveling up 
            _hud.SetXp(0);

            // we don't want to set the next milestone if we just reached the final level (there 
            // are no more milestones)
            if (_currentLevel < _levelMilestones.Length)
            {
                _nextLevelScoreMilestone = _levelMilestones[_currentLevel];
                _previousLevelScoreMilestone = _levelMilestones[_previousLevel];
            }
            GameObject levelUpUI = Instantiate(_levelUpUIPrefab);
        }

        
    }

    /**
      * Returns the bounding box of the camera viewport in world space coordinates 
      */
    private Bounds GetViewportBounds() {
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
     * Returns a random spawnable position outside of the viewport
     */
    private Vector3 GetRandomSpawnPointOutsideViewport() {
        var tileMap = _floor;

        // get the bounding world coordinates of the tilemap
        var tilemapBounds = tileMap.localBounds;
        var tilemapWorldBounds = new Bounds(tilemapBounds.center + tileMap.transform.position, tilemapBounds.size);

        // convert to bounds
        var viewportBounds = GetViewportBounds();

       // randomly choose a coordinate until one is found that is outside the viewport
        Vector3 floorSpawnCoord;
        do {
            floorSpawnCoord = new Vector3(
            Random.Range(tilemapWorldBounds.min.x, tilemapWorldBounds.max.x), 
            Random.Range(tilemapWorldBounds.min.y, tilemapWorldBounds.max.y), 0.0f);

            // loop until a coordinate is chosen outside viewport
        } while (viewportBounds.Contains(floorSpawnCoord));

        return floorSpawnCoord;
    }

    private void SpawnEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab);
        enemy.GetComponent<Enemy>().hitpoints = _maxHitPoints;
        enemy.transform.parent = _levelContainer.transform;

        enemy.transform.position = GetRandomSpawnPointOutsideViewport();
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
