using System;
using System.Collections;
using System.Net.Http;
using Sentry;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    private DemoConfiguration _demoConfig;
    
    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<BattleSceneManager>();
        _demoConfig = Resources.Load("DemoConfig") as DemoConfiguration;
        
        _submitButton.onClick.AddListener(OnSubmit);
    }

    public void OnEnable()
    {
        if (_demoConfig == null 
            || !_demoConfig.Enabled 
            || string.IsNullOrEmpty(_demoConfig.ApiUrl) 
            || string.IsNullOrEmpty(_demoConfig.Psk))
        {
            gameObject.SetActive(false);
        }
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
        
        using var www = UnityWebRequest.Post(_demoConfig.ApiUrl, json, "application/json");
        
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
