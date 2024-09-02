using UnityEngine;
using UnityEngine.UI;

/**
 * Encapsulates behavior of LevelUpUI prefab
 */
public class LevelUpUI : MonoBehaviour
{
    // fyi: title -> upgrade name, description -> level, stats -> description
    // leveling up an upgrade, changes the stats to new level, increases the level #

    [SerializeField]
    private LevelOptionUI _levelOption1;

    [SerializeField]
    private LevelOptionUI _levelOption2;

    private Button _option1Button;
    private Button _option2Button;

    void Awake()
    {
        _option1Button = _levelOption1.GetComponent<Button>();
        _option2Button = _levelOption2.GetComponent<Button>();
    }

    void OnEnable()
    {
        // pause the game
        Time.timeScale = 0;

        // get upgrade manager
        (UpgradePathBase upgradeChoice1, UpgradePathBase upgradeChoice2) =
            UpgradeManager.Instance.GetRandomUpgradePaths();

        SetLevelOptionUI(upgradeChoice1, upgradeChoice2);

        _option1Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice1));
        _option2Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice2));
    }

    void OnDisable()
    {
        _option1Button.onClick.RemoveAllListeners();
        _option2Button.onClick.RemoveAllListeners();
    }

    /**
     * Given a set of option choices, update the UI accordingly
     */
    void SetLevelOptionUI(UpgradePathBase option1, UpgradePathBase option2)
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

    void SelectUpgrade(UpgradePathBase selectedUpgrade)
    {
        UpgradeManager.Instance.LevelUpUpgradePath(selectedUpgrade);

        // resume the game and exit the level up popup
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
