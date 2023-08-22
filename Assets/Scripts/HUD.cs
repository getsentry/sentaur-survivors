using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

/**
 * Heads-up display (HUD) for the game.
 */
public class HUD : MonoBehaviour
{

    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _timeElapsedText;
    private TextMeshProUGUI _gameOverText;
    private TextMeshProUGUI _currentLevelText;

    private XpBar _xpBar;
    
    void Awake() {
        // get score text component from child
        _scoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        _timeElapsedText = transform.Find("TimeElapsed").GetComponent<TextMeshProUGUI>();
        _gameOverText = transform.Find("GameOver").GetComponent<TextMeshProUGUI>();
        _currentLevelText = transform.Find("XpBar").GetComponentInChildren<TextMeshProUGUI>();

        _xpBar = transform.Find("XpBar").GetComponent<XpBar>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // get time elapsed since game start in mm:ss format
        var timeElapsed = Time.timeSinceLevelLoad;
        var minutes = Mathf.FloorToInt(timeElapsed / 60.0f);
        var seconds = Mathf.FloorToInt(timeElapsed % 60.0f);
        _timeElapsedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetScore(int score) {
        _scoreText.text = "Score: " + score;
    }

    public void SetXp(float xp) {
        _xpBar.SetXp(xp);
    }

    public void ShowGameOver() {
        _gameOverText.enabled = true;
    }

    public void SetCurrentLevel(int level) {
        _currentLevelText.text = "Level " + level;
    }
}
