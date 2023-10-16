using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Encapsulates behavior of LevelUpUI prefab
 */
public class LevelUpUI : MonoBehaviour
{

    // fyi: title -> upgrade name, description -> level, stats -> description 
    // leveling up an upgrade, changes the stats to new level, increases the level #

    public static List<string> AvailableProjectileUpgrades = new List<string>{
        "count++", "cooldown++", "damage++", "dart", "raven", "starfish"
    };

    public static Dictionary<string, UpgradePath> UpgradeData = new Dictionary<string, UpgradePath>{
        {
            "count++",
            new UpgradePath("count++", new List<string>{"2 of each projectile", "3 of each projectile", "5 of each projectile!"})
        },
        {
            "cooldown++",
            new UpgradePath("cooldown++", new List<string>{"-20% cooldown time", "-25% cooldown time", "-50% cooldown time!"})
        },
        {
            "damage++",
            new UpgradePath("damage++", new List<string>{"+30% damage", "+60% damage", "+100% damage!"})
        },
        {
            "dart",
            new UpgradePath("dart", new List<string>{"extra dart that fires behind you", "+50% damage", "3 darts firing behind and +33% damage!"})
        },
        {
            "raven",
            new UpgradePath("raven", new List<string>{"heat-seeking bomb targets closest enemy", "gain an additional raven", "+33% damage and -20% cooldown!"})
        },
        {
            "starfish",
            new UpgradePath("starfish", new List<string>{"orbits around you, wreaking havoc", "+20% orbit duration", "+50% orbit duration and -30% cooldown!"})
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

    void Awake() {
    }

    // Start is called before the first frame update
    void Start()
    {
        // pause the game
        Time.timeScale = 0;

        (int upgradeChoice1, int upgradeChoice2) = GetRandomValidChoice();

        SetLevelOptionUI(upgradeChoice1, upgradeChoice2);

        var option1Button = _levelOption1.GetComponent<Button>();
        var option2Button = _levelOption2.GetComponent<Button>();

        option1Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice1));
        option2Button.onClick.AddListener(() => SelectUpgrade(upgradeChoice2));
    }

    /**
     * Returns a tuple of random level upgrade indices that are valid
     */
    (int, int) GetRandomValidChoice() {
        int option1 = Random.Range(0, AvailableProjectileUpgrades.Count);
        int option2;

        // select a second option that is different from the first option if the number of available
        // projectiles is greater than 1
        do 
        {
            option2 = Random.Range(0, AvailableProjectileUpgrades.Count);
        } while (AvailableProjectileUpgrades.Count > 1 && option2 == option1);
        return (option1, option2);
    }

    /**
     * Given a set of option choices, update the UI accordingly
     */
    void SetLevelOptionUI(int option1, int option2) 
    {
        string optionTitle = AvailableProjectileUpgrades[option1];
        int optionLevel = UpgradeData[optionTitle].CurrentLevel + 1;
        string optionStats = UpgradeData[optionTitle].GetLevelStats(optionLevel);
        _levelOption1.Set(title: optionTitle, description: "Level " + optionLevel, stats: optionStats);

        if (option1 == option2) {
            _levelOption2.Set("ALL OTHER UPGRADES MAXED OUT", "", "");
        } else {
            optionTitle = AvailableProjectileUpgrades[option2];
            optionLevel = UpgradeData[optionTitle].CurrentLevel + 1;
            optionStats = UpgradeData[optionTitle].GetLevelStats(optionLevel);
            _levelOption2.Set(title: optionTitle, description: "Level " + optionLevel, stats: optionStats);
        }
    }

    void SelectUpgrade(int selectedUpgradeIndex)
    {
        string selectedUpgrade = AvailableProjectileUpgrades[selectedUpgradeIndex];
        
        UpgradeData[selectedUpgrade].LevelUp(); // level up the selected upgrade

        if (UpgradeData[selectedUpgrade].CurrentLevel == MAX_LEVEL) 
        {
            // take the upgrade out of the pool if it's maxed out
            for (int i = 0; i < AvailableProjectileUpgrades.Count; i++)
            {
                if (AvailableProjectileUpgrades[i] == selectedUpgrade) 
                {
                    AvailableProjectileUpgrades.RemoveAt(i);
                    break;
                }
            }
        }

        int newLevel = UpgradeData[selectedUpgrade].CurrentLevel;
     
        switch(selectedUpgrade) 
        {
            case "count++": 
                ProjectileBase.UpgradeCount(newLevel);
                // add upgrade to the active upgrades container in the hud
                // i think we need an EventListener like we do in Pickup.cs:84, maybe?
                // _activePickupContainer.transform.Find("Count").gameObject.SetActive(true);
                break;
            case "cooldown++":
                ProjectileBase.UpgradeCooldown(newLevel);
                break;
            case "damage++":
                ProjectileBase.UpgradeDamage(newLevel);
                break;
            case "dart":
                Dart.UpgradeDart(newLevel);
                break;
            case "raven":
                Raven.UpgradeRaven(newLevel);
                break;
            case "starfish":
                Starfish.UpgradeStarfish(newLevel);
                break;
            default: break;
        }

        // resume the game and exit the level up popup
        Time.timeScale = 1; 
        Destroy(gameObject);
        
    }
}
