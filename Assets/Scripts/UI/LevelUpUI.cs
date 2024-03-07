using System.Collections.Generic;
using Sentry;
using UnityEngine;
using UnityEngine.UI;

/**
 * Encapsulates behavior of LevelUpUI prefab
 */
public class LevelUpUI : MonoBehaviour
{
    // fyi: title -> upgrade name, description -> level, stats -> description
    // leveling up an upgrade, changes the stats to new level, increases the level #

    public static List<UpgradeType> _availableUpgradeTypes = new List<UpgradeType>
    {
        UpgradeType.CountUp,
        UpgradeType.CooldownDown,
        UpgradeType.DamageUp,
        UpgradeType.Dart,
        UpgradeType.Raven,
        UpgradeType.Starfish
    };

    public static Dictionary<UpgradeType, UpgradePath> _upgradeData = new Dictionary<
        UpgradeType,
        UpgradePath
    >
    {
        {
            UpgradeType.CountUp,
            new UpgradePath(
                new List<string>
                {
                    "2 of each projectile",
                    "3 of each projectile",
                    "5 of each projectile!"
                }
            )
        },
        {
            UpgradeType.CooldownDown,
            new UpgradePath(
                new List<string>
                {
                    "-20% cooldown time",
                    "-25% cooldown time",
                    "-50% cooldown time!"
                }
            )
        },
        {
            UpgradeType.DamageUp,
            new UpgradePath(new List<string> { "+30% damage", "+60% damage", "+100% damage!" })
        },
        {
            UpgradeType.Dart,
            new UpgradePath(
                new List<string>
                {
                    "extra dart that fires behind you",
                    "+50% damage",
                    "3 darts firing behind and +33% damage!"
                }
            )
        },
        {
            UpgradeType.Raven,
            new UpgradePath(
                new List<string>
                {
                    "heat-seeking bomb targets closest enemy",
                    "gain an additional raven",
                    "+33% damage and -20% cooldown!"
                }
            )
        },
        {
            UpgradeType.Starfish,
            new UpgradePath(
                new List<string>
                {
                    "orbits around you, wreaking havoc",
                    "+20% orbit duration",
                    "+50% orbit duration and -30% cooldown!"
                }
            )
        }
    };

    private int MAX_LEVEL = 3;

    [SerializeField]
    private LevelOption _levelOption1;

    [SerializeField]
    private LevelOption _levelOption2;

    // this is in GameManager.cs, but idk how to access it from here
    // [SerializeField]
    // [Tooltip("The parent UI element containing the active upgrades")]
    // private GameObject _activeUpgradesContainer;

    void Awake() { }

    // Start is called before the first frame update
    void Start()
    {
        // pause the game
        Time.timeScale = 0;

        (UpgradeType upgradeChoice1, UpgradeType upgradeChoice2) = GetRandomUpgradeChoices();

        SetLevelOptionUI(upgradeChoice1, upgradeChoice2);

        var option1Button = _levelOption1.GetComponent<Button>();
        var option2Button = _levelOption2.GetComponent<Button>();

        option1Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice1));
        option2Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice2));
    }

    /**
     * Returns a tuple of random level upgrade indices that are valid
     */
    (UpgradeType, UpgradeType) GetRandomUpgradeChoices()
    {
        int option1 = Random.Range(0, _availableUpgradeTypes.Count);
        int option2;

        // select a second option that is different from the first option if the number of available
        // projectiles is greater than 1
        do
        {
            option2 = Random.Range(0, _availableUpgradeTypes.Count);
        } while (_availableUpgradeTypes.Count > 1 && option2 == option1);

        UpgradeType upgradeType1 = _availableUpgradeTypes[option1];
        UpgradeType upgradeType2 = _availableUpgradeTypes[option2];
        return (upgradeType1, upgradeType2);
    }

    /**
     * Given a set of option choices, update the UI accordingly
     */
    void SetLevelOptionUI(UpgradeType option1, UpgradeType option2)
    {
        UpgradeType optionTitle = option1;
        int optionLevel = _upgradeData[optionTitle].CurrentLevel + 1;
        string optionStats = _upgradeData[optionTitle].GetLevelStats(optionLevel);
        _levelOption1.Set(
            upgradeType: optionTitle,
            description: "Level " + optionLevel,
            stats: optionStats
        );

        if (option1 == option2)
        {
            _levelOption2.SetMaxedOut();
        }
        else
        {
            optionTitle = option2;
            optionLevel = _upgradeData[optionTitle].CurrentLevel + 1;
            optionStats = _upgradeData[optionTitle].GetLevelStats(optionLevel);
            _levelOption2.Set(
                upgradeType: optionTitle,
                description: "Level " + optionLevel,
                stats: optionStats
            );
        }
    }

    void SelectUpgrade(UpgradeType selectedUpgradeType)
    {
        _upgradeData[selectedUpgradeType].LevelUp(); // level up the selected upgrade

        if (_upgradeData[selectedUpgradeType].CurrentLevel == MAX_LEVEL)
        {
            // take the upgrade out of the pool if it's maxed out
            for (int i = 0; i < _availableUpgradeTypes.Count; i++)
            {
                if (_availableUpgradeTypes[i] == selectedUpgradeType)
                {
                    _availableUpgradeTypes.RemoveAt(i);
                    break;
                }
            }
        }

        int newLevel = _upgradeData[selectedUpgradeType].CurrentLevel;

        var upgradeEvent = new UpgradeEventData(selectedUpgradeType, newLevel);
        EventManager.TriggerEvent("UpgradeChosen", new EventData(upgradeEvent));

        SentrySdk.Metrics.Increment(
            "upgrade_selected",
            tags: new Dictionary<string, string> { { "type", selectedUpgradeType.ToString() } }
        );

        // resume the game and exit the level up popup
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
