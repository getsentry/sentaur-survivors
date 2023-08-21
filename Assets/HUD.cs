using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class HUD : MonoBehaviour
{

    private TextMeshProUGUI _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        // get score text component from child
        _scoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();

        
        EventManager.AddListener("ScoreChange", (eventData) => {
            OnScoreChange((int)eventData.Data);
        });    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnScoreChange(int score)
    {
        _scoreText.text = "Score: " + score;
    }
}
