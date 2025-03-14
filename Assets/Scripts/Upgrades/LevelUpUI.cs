using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    /**
 * Encapsulates behavior of LevelUpUI prefab
 */
    public class LevelUpUI : MonoBehaviour
    {
        // fyi: title -> upgrade name, description -> level, stats -> description
        // leveling up an upgrade, changes the stats to new level, increases the level #

        [SerializeField] private LevelOptionUI _levelOption1;
        [SerializeField] private LevelOptionUI _levelOption2;

        private Button _option1Button;
        private Button _option2Button;
        private Button _highlightedButton;

        private void Awake()
        {
            _option1Button = _levelOption1.GetComponent<Button>();
            _option2Button = _levelOption2.GetComponent<Button>();
        }

        private void OnEnable()
        {
            // Pause the game
            Time.timeScale = 0;

            var paths = UpgradeManager.Instance.GetRandomUpgradePaths(2);
            var upgradeChoice1 = paths[0];
            var upgradeChoice2 = paths[1];

            SetLevelOptionUI(upgradeChoice1, upgradeChoice2);

            _option1Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice1));
            _option2Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice2));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SetHighlightedButton(_option1Button);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SetHighlightedButton(_option2Button);
            }
        
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _highlightedButton.onClick.Invoke();
            }
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
            UpgradeManager.Instance.LevelUpUpgradePath(selectedUpgrade);

            // Resume the game and exit the level up popup
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}
