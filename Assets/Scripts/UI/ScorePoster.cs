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
    private GameObject _root;
    [SerializeField] 
    private TMP_InputField _nameField;
    [SerializeField] 
    private Button _submitButton;
    
    private BattleSceneManager _gameManager;
    private DemoConfiguration _demoConfig;
    private TextMeshProUGUI _buttonText;

    private string _jwtToken;
    
    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<BattleSceneManager>();
        _demoConfig = Resources.Load("DemoConfig") as DemoConfiguration;
        _buttonText = _submitButton.GetComponentInChildren<TextMeshProUGUI>();
        
        _submitButton.onClick.AddListener(OnSubmit);
    }

    public void Start()
    {
        // Doing this in start so everything else can awake
        if (_demoConfig != null && _demoConfig.Enabled && !string.IsNullOrEmpty(_demoConfig.ApiUrl))
        {
            StartCoroutine(Login());
        }
    }

    public void Enable()
    {
        // If we did not manage to login during `Awake` (which means scene loading) then we do not display the upload screen
        if (_jwtToken != null)
        {
            _root.SetActive(true);
        }
    }
    
    IEnumerator Login()
    {
        var json = JsonUtility.ToJson(_demoConfig.User);
        
        using var www = UnityWebRequest.Post(_demoConfig.ApiUrl + "/token", json, "application/json");
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Login to leaderboard successful.");
            _jwtToken = www.downloadHandler.text.Replace("\"", "");
        }
        else
        {
            Debug.Log("Login to leaderboard failed.");
            _jwtToken = null;
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
            Duration = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString(),
            Score = _gameManager.GetScore(),
            Timestamp = DateTime.Now.ToString("o")
        };

        var json = JsonUtility.ToJson(score);

        var uploadTransaction = SentrySdk.StartTransaction("scoreposter", "upload");
        SentrySdk.ConfigureScope(scope => scope.Transaction = uploadTransaction);
        
        using var www = UnityWebRequest.Post(_demoConfig.ApiUrl + "/score", json, "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + _jwtToken);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Uploading score to leaderboard failed.");
            SentrySdk.CaptureException(new HttpRequestException("Failed to upload score."));
            
            _buttonText.text = "Retry";
            uploadTransaction.Finish(SpanStatus.Unavailable);
        }
        else
        {
            Debug.Log("Uploading score to leaderboard was successful.");
            _submitButton.interactable = false;
            _buttonText.text = "Posted!";
            uploadTransaction.Finish(SpanStatus.Ok);
        }
    }
}
