using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HUD : MonoBehaviour
{

    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        // get score text component from child
        _scoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        _timeElapsed = transform.Find("TimeElapsed").GetComponent<TextMeshProUGUI>();

        
        EventManager.AddListener("ScoreChange", (eventData) => {
            OnScoreChange((int)eventData.Data);
        });
    }

    // Update is called once per frame
    void Update()
    {
        // get time elapsed since game start in mm:ss format
        var timeElapsed = Time.timeSinceLevelLoad;
        var minutes = Mathf.FloorToInt(timeElapsed / 60.0f);
        var seconds = Mathf.FloorToInt(timeElapsed % 60.0f);
        _timeElapsed.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnScoreChange(int score)
    {
        _scoreText.text = "Score: " + score;
    }
}
