using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Heads-up display (HUD) for the game.
 */
public class HUD : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _timeElapsedText;
    private TextMeshProUGUI _gameOverText;
    private TextMeshProUGUI _currentLevelText;

    [SerializeField]
    private ScorePoster _scorePoster;
    [SerializeField]
    private GameObject _tryAgain;

    [SerializeField]
    private GameObject _quit;

    private XpBar _xpBar;

    void Awake()
    {
        // get score text component from child
        _scoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        _timeElapsedText = transform.Find("TimeElapsed").GetComponent<TextMeshProUGUI>();
        _gameOverText = transform.Find("GameOver").GetComponent<TextMeshProUGUI>();
        _currentLevelText = transform.Find("XpBar").GetComponentInChildren<TextMeshProUGUI>();

        _xpBar = transform.Find("XpBar").GetComponent<XpBar>();

        var _tryAgainButton = _tryAgain.GetComponent<Button>();
        _tryAgainButton.onClick.AddListener(() =>
        {
            EventManager.TriggerEvent("TryAgain");
        });

        var _quitButton = _quit.GetComponent<Button>();
        _quitButton.onClick.AddListener(() =>
        {
            EventManager.TriggerEvent("Quit");
        });
    }

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // get time elapsed since game start in mm:ss format
        var timeElapsed = Time.timeSinceLevelLoad;
        var minutes = Mathf.FloorToInt(timeElapsed / 60.0f);
        var seconds = Mathf.FloorToInt(timeElapsed % 60.0f);
        _timeElapsedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void SetXp(float xp)
    {
        _xpBar.SetXp(xp);
    }

    public void ShowPause()
    {
        _gameOverText.text = "PAUSED";
        _gameOverText.enabled = true;

        _quit.SetActive(true);
    }

    public void HidePause()
    {
        _gameOverText.enabled = false;
        _quit.SetActive(false);
    }

    public void ShowGameOver()
    {
        _gameOverText.text = "GAME OVER";
        _gameOverText.enabled = true;
        
        _quit.SetActive(true);
        _tryAgain.SetActive(true);
        _scorePoster.Enable();
    }

    public void SetCurrentLevel(int level)
    {
        _currentLevelText.text = "Level " + (level + 1);
    }
}
