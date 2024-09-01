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

    public static void Reset()
    {
        /*

        _upgradeData = new Dictionary<UpgradeType, UpgradePath>
        {
            {
                UpgradeType.CountUp,
                new UpgradePath(
                    new List<string>
                    {
                        "+1 of each projectile",
                        "+1 of each projectile",
                        "+2 of each projectile!"
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
                        "+1 dart that fires behind you",
                        "+50% damage",
                        "+2 dart firing behind and +33% damage!"
                    }
                )
            },
            {
                UpgradeType.Raven,
                new UpgradePath(
                    new List<string>
                    {
                        "heat-seeking bomb targets closest enemy",
                        "+33% damage and +20% cooldown!",
                        "+60% area of effect"
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
            },
            {
                UpgradeType.Schnitzel,
                new UpgradePath(
                    new List<string>
                    {
                        "its like an axe",
                        "+40% area of effect",
                        "+30% area of effect"
                    }
                )
            }
        };*/
    }

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
        (UpgradeBase upgradeChoice1, UpgradeBase upgradeChoice2) =
            UpgradeManager.Instance.GetRandomUpgradeChoices();

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
    void SetLevelOptionUI(UpgradeBase option1, UpgradeBase option2)
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

    void SelectUpgrade(UpgradeBase selectedUpgrade)
    {
        UpgradeManager.Instance.UpgradePath(selectedUpgrade);

        // resume the game and exit the level up popup
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
