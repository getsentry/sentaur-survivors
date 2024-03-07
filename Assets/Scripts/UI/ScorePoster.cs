using System;
using System.Collections;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Sentry;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.Json;

[Serializable]
public class ScoreEntry
{
    public Guid Key;
    public string Name;
    public string Email;
    public string Duration;
    public int Score;
    public string Timestamp;
}

public class ScorePoster : MonoBehaviour
{
    [SerializeField] 
    private TMP_InputField _nameField;
    [SerializeField] 
    private TMP_InputField _emailField;
    [SerializeField] 
    private Button _submitButton;

    private BattleSceneManager _gameManager;
    
    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<BattleSceneManager>();
        _submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        StartCoroutine(UploadScore());        
    }

    IEnumerator UploadScore()
    {
        var score = new ScoreEntry
        {
            Key = new Guid(),
            Name = _nameField.text,
            Email = _emailField.text,
            Duration = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString(),
            Score = _gameManager.GetScore(),
            Timestamp = DateTime.Now.ToString("o")
        };

        var json = JsonUtility.ToJson(score);
        
        using var www = UnityWebRequest.Post("http://localhost:5203/score", json, "application/json");
        
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to upload score.");
            SentrySdk.CaptureException(new HttpRequestException("Failed to upload score."));
            
            var buttonText = _submitButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "Retry";
        }
        else
        {
            _submitButton.interactable = false;
            Debug.Log("Score uploaded.");
        }
    }
}
