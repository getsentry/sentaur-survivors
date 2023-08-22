using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The enemy prefab to spawn")]
    private GameObject _enemyPrefab;

    [SerializeField]
    [Tooltip("How frequently enemies spawn (in seconds)")]
    private float _enemySpawnRate = 2.0f;

    [SerializeField]
    [Tooltip("The level up UI prefab to spawn")]
    private GameObject _levelUpUIPrefab;

    [SerializeField]
    [Tooltip("Starting XP")]
    private float _xp = 0;

    // the player's accumulated score so far
    private int _score = 0;

    // the score a player must get to for the next weapon upgrade
    private int _nextLevelScoreMilestone;
    // trackinf the previous level score milestone for xp bar
    private int _previousLevelScoreMilestone;
    private int[] _levelMilestones = {10, 50, 100, 200, 300, 400, 500, 600, 700}; // TODO: update
    private int _currentLevel = 0;
    private int _previousLevel = 0;

    private float _lastEnemySpawnTime = 0.0f;

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

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(_enemyPrefab as GameObject);
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
}
