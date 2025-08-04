using Sentry;
using Sentry.Unity;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
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
    private HttpClient _httpClient;

    private void Awake()
    {
        _gameManager = GameObject.Find("BattleSceneManager").GetComponent<BattleSceneManager>();
        _demoConfig = Resources.Load("DemoConfig") as DemoConfiguration;
        _buttonText = _submitButton.GetComponentInChildren<TextMeshProUGUI>();

        _submitButton.onClick.AddListener(OnSubmit);
        
    }

    public void Start()
    {
        if (_demoConfig != null && _demoConfig.Enabled && !string.IsNullOrEmpty(_demoConfig.ApiUrl))
        {
            _httpClient = new HttpClient(new SentryHttpMessageHandler());
            _ = LoginAsync();
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

    private async Task LoginAsync()
    {
        var transaction = SentrySdk.StartTransaction("scoreposter", "login");
        SentrySdk.ConfigureScope(scope => scope.Transaction = transaction);
        
        try
        {
            var json = JsonUtility.ToJson(_demoConfig.User);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(_demoConfig.ApiUrl + "/token", content);
            if (response.IsSuccessStatusCode)
            {
                Debug.Log("Login to leaderboard successful.");
                transaction.Finish(SpanStatus.Ok);
                _jwtToken = (await response.Content.ReadAsStringAsync()).Replace("\"", "");
            }
            else
            {
                Debug.Log("Login to leaderboard failed.");
                transaction.Finish(SpanStatus.Unavailable);
                _jwtToken = null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login failed: {ex.Message}");
            transaction.Finish(SpanStatus.InternalError);
            _jwtToken = null;
        }
    }

    private void OnSubmit()
    {
        _ = UploadScoreAsync();
    }

    private async Task UploadScoreAsync()
    {
        var score = new ScoreEntry
        {
            Key = Guid.NewGuid(),
            Name = _nameField.text,
            Duration = TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString(),
            Score = _gameManager.GetScore(),
            Timestamp = DateTime.Now.ToString("o")
        };

        var json = JsonUtility.ToJson(score);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var uploadTransaction = SentrySdk.StartTransaction("scoreposter", "upload");
        SentrySdk.ConfigureScope(scope => scope.Transaction = uploadTransaction);

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);
            var response = await _httpClient.PostAsync(_demoConfig.ApiUrl + "/score", content);

            if (!response.IsSuccessStatusCode)
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
        catch (Exception ex)
        {
            Debug.LogError($"Score upload failed: {ex.Message}");
            _buttonText.text = "Retry";
            uploadTransaction.Finish(SpanStatus.InternalError);
        }
    }
}
