using System;
using System.Collections;
using System.Collections.Generic;
using Sentry;
using Sentry.Unity;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Upgrades
{
    /**
 * Encapsulates behavior of LevelUpUI prefab
 */
    public class LevelUpUI : MonoBehaviour
    {
        private InputAction _navigateAction;
        private InputAction _submitAction;
        
        // fyi: title -> upgrade name, description -> level, stats -> description
        // leveling up an upgrade, changes the stats to new level, increases the level #

        [SerializeField] private LevelOptionUI _levelOption1;
        [SerializeField] private LevelOptionUI _levelOption2;

        private DemoConfiguration _demoConfig;
        private BattleSceneManager _gameManager;
        
        private Button _option1Button;
        private Button _option2Button;
        private Button _highlightedButton;

        private void Awake()
        {
            _demoConfig = Resources.Load("DemoConfig") as DemoConfiguration;
            _gameManager = GameObject.Find("BattleSceneManager").GetComponent<BattleSceneManager>();
            
            _navigateAction = InputSystem.actions.FindAction("Navigate");
            _submitAction = InputSystem.actions.FindAction("Submit");
            
            _option1Button = _levelOption1.GetComponent<Button>();
            _option2Button = _levelOption2.GetComponent<Button>();
        }

        private void OnEnable()
        {
            InputSystem.actions.FindActionMap("Player").Disable();
            InputSystem.actions.FindActionMap("UI").Enable();
            
            // Pause the game
            Time.timeScale = 0;

            List<UpgradePathBase> paths = null;
            if (_demoConfig != null && _demoConfig.FetchUpgradeFromServer)
            {
                paths = GetUpgrades();
            }

            paths ??= UpgradeManager.Instance.GetRandomUpgradePaths(2);
            if (paths == null || paths.Count == 0)
            {
                Debug.LogWarning("No upgrade paths available. Everything fully upgraded?");
                Time.timeScale = 1;
                gameObject.SetActive(false);
                return;
            }

            
            var upgradeChoice1 = paths[0];
            var upgradeChoice2 = paths.Count > 1 ? paths[1] : paths[0]; // In case there is only one upgrade left
            
            SetLevelOptionUI(upgradeChoice1, upgradeChoice2);

            _option1Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice1));
            _option2Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice2));

            if (_demoConfig != null && _demoConfig.AutoPlay)
            {
                StartCoroutine(SelectSomething());
            }
        }

        private IEnumerator SelectSomething()
        {
            var delay = Random.value;
            Debug.Log($"Starting to select in {delay} seconds");
            yield return new WaitForSecondsRealtime(delay);

            Debug.Log("Done waiting");
            
            if (Random.value > 0.5f)
            {
                Debug.Log("Selected left");
                SetHighlightedButton(_option1Button);
            }
            else
            {
                Debug.Log("Selected right");
                SetHighlightedButton(_option2Button);
            }

            yield return new WaitForSecondsRealtime(Random.value);
            
            Debug.Log("Clicking the highlighted button");
            _highlightedButton?.GetComponent<Button>().onClick.Invoke();
        }
        
        public void OnNavigate()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            if (!_navigateAction.WasPressedThisFrame())
            {
                return;
            }
            
            var direction = _navigateAction.ReadValue<Vector2>();
            if (direction.x < 0)
            {
                SetHighlightedButton(_option1Button);
            }
            else if (direction.x > 0)
            {
                SetHighlightedButton(_option2Button);
            }
        }
        
        public void OnSubmit()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            if (!_submitAction.IsPressed())
            {
                return;
            }
            
            _highlightedButton?.GetComponent<Button>().onClick.Invoke();
        }

        public void SetHighlightedButton(Button button)
        {
            _option1Button.GetComponent<Highlighter>().Highlight(false);
            _option2Button.GetComponent<Highlighter>().Highlight(false);
        
            button.GetComponent<Highlighter>().Highlight();
            _highlightedButton = button;
        }

        private void OnDisable()
        {
            _option1Button.onClick.RemoveAllListeners();
            _option2Button.onClick.RemoveAllListeners();
        }

        // Given a set of option choices, update the UI accordingly
        private void SetLevelOptionUI(UpgradePathBase option1, UpgradePathBase option2)
        {
            _levelOption1.Set(
                title: option1.Title,
                description: "Level " + option1.NextLevel,
                stats: option1.NextDescription,
                icon: option1.Icon
            );

            if (option1 == option2)
            {
                _levelOption2.SetMaxedOut();
            }
            else
            {
                _levelOption2.Set(
                    title: option2.Title,
                    description: "Level " + option2.NextLevel,
                    stats: option2.NextDescription,
                    icon: option2.Icon
                );
            }
        }

        private void SelectUpgrade(UpgradePathBase selectedUpgrade)
        {
            InputSystem.actions.FindActionMap("Player").Enable();
            InputSystem.actions.FindActionMap("UI").Disable();
            
            UpgradeManager.Instance.LevelUpUpgradePath(selectedUpgrade);

            // Resume the game and exit the level up popup
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
        
        private List<UpgradePathBase> GetUpgrades()
        {
            var fetchTransaction = SentrySdk.StartTransaction("fetch_upgrades", "http.client");
            SentrySdk.ConfigureScope(scope => scope.Transaction = fetchTransaction);
            
            try
            {
                var responseContent = FetchUpgradeDataFromServer(fetchTransaction);
                var upgrades = ParseUpgradeData(responseContent, fetchTransaction);
                
                if (upgrades == null)
                {
                    Debug.LogWarning("Upgrade parsing failed, falling back to local upgrades");
                    fetchTransaction.Finish(SpanStatus.Ok);
                    return null;
                }
                
                fetchTransaction.Finish(SpanStatus.Ok);
                return upgrades;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error fetching upgrades from server: {ex.Message}");
                SentrySdk.CaptureException(ex);
                fetchTransaction.Finish(SpanStatus.InternalError);
                
                return null;
            }
        }
        
        private string FetchUpgradeDataFromServer(ITransactionTracer transaction)
        {
            var processDataSpan = transaction.StartChild("task", "process_level_data");
            
            var currentLevel = _gameManager.GetCurrentLevel();
            
            System.Threading.Tasks.Task.Delay((int)(Random.value * 100)).Wait(); // Simulate some work to be done
        
            processDataSpan.Finish();
        
            const string domain = "https://aspnetcore.empower-plant.com";
            const string upgradesEndpoint = "/reviews";
            var upgradesURL = $"{domain}{upgradesEndpoint}?currentLevel={currentLevel}";
        
            var client = new System.Net.Http.HttpClient(new SentryHttpMessageHandler());
            client.Timeout = TimeSpan.FromSeconds(3);

            try
            {
                var response = client.GetAsync(upgradesURL).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Server returned error status: {response.StatusCode}");
                }
                
                Debug.Log("Successfully fetched upgrade data from server");
                var responseContent = response.Content.ReadAsStringAsync().Result;
                
                return responseContent;
            }
            finally
            {
                client.Dispose();
            }
        }
        
        private List<UpgradePathBase> ParseUpgradeData(string responseContent, ITransactionTracer transaction)
        {
            var parseSpan = transaction.StartChild("task", "parse_upgrade_data");

            try
            {
                var serverResponse = JsonUtility.FromJson<ServerUpgradeResponse>(responseContent);
                var upgradePaths = new List<UpgradePathBase>(serverResponse.upgrades.Length);

                for (var i = 0; i < serverResponse.upgrades.Length; i++)
                {
                    var serverUpgrade = serverResponse.upgrades[i];

                    // We need to map server upgrade data to actual UpgradePathBase instances
                    // We'd need a sort of factory method or some mapping logic here
                }

                parseSpan.Finish(SpanStatus.Ok);
                return upgradePaths;
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to parse the upgrade data.");
                SentrySdk.CaptureException(ex);
                parseSpan.Finish(SpanStatus.InternalError);
                return null;
            }
        }
        
        [Serializable]
        public class ServerUpgradeResponse
        {
            public ServerUpgrade[] upgrades;
        }
        
        [Serializable]
        public class ServerUpgrade
        {
        }
    }
}
