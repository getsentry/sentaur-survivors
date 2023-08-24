using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpProjectile : MonoBehaviour
{

    // fyi: title -> upgrade name, description -> level, stats -> description 
    // leveling up an upgrade, changes the stats to new level, increases the level #

    public static List<string> AvailableProjectileUpgrades = new List<string>{
        "count++", "speed++", "damage++", "raven", "starfish"
    };

    public static Dictionary<string, Upgrade> UpgradeData = new Dictionary<string, Upgrade>{
        {
            "count++",
            new Upgrade("count++", new List<string>{"3 darts", "5 darts", "7 darts!)"})
        },
        {
            "speed++",
            new Upgrade("speed++", new List<string>{"+50% dart fire rate", "+100% dart fire rate", "+250% dart fire rate!"})
        },
        {
            "damage++",
            new Upgrade("damage++", new List<string>{"+100% dart damage", "+200% dart damage", "+300% dart damage!"})
        },
        {
            "raven",
            new Upgrade("raven", new List<string>{"targets nearest enemy", "targets more frequently with more damage", "targets two enemies and deals even more damage!"})
        },
        {
            "starfish",
            new Upgrade("starfish", new List<string>{"orbits around you, damaging enemies", "orbits for longer and deals more damage", "orbits faster and deals more damage!"})
        }
    };

   private int MAX_LEVEL = 3;

    // these are equivalent to the index of the option in AvailableProjectileUpgrades
    private int option1;
    private int option2;

    [SerializeField]
    private LevelOption _levelOption1;

    [SerializeField]
    private LevelOption _levelOption2;

    void Awake() {
    }

    // Start is called before the first frame update
    void Start()
    {
        // pause the game
        Time.timeScale = 0;

        RandomizeOptions();

        var option1Button = _levelOption1.GetComponent<Button>();
        var option2Button = _levelOption2.GetComponent<Button>();

        option1Button.onClick.AddListener(() => SelectUpgrade(1));
        option2Button.onClick.AddListener(() => SelectUpgrade(2));
    }

    void RandomizeOptions() 
    {
        option1 = Random.Range(0, AvailableProjectileUpgrades.Count);

        // select a second option that is different from the first option if the number of available
        // projectiles is greater than 1
        do 
        {
            option2 = Random.Range(0, AvailableProjectileUpgrades.Count);
        } while (AvailableProjectileUpgrades.Count > 1 && option2 == option1);

        string optionTitle = AvailableProjectileUpgrades[option1];
        int optionLevel = UpgradeData[optionTitle].CurrentLevel + 1;
        string optionStats = UpgradeData[optionTitle].GetLevelStats(optionLevel);
        // Debug.Log("LevelUpProjectile.RandomizeOptions: option1 is " + option1Description);   
        _levelOption1.Set(title: optionTitle, description: "Level " + optionLevel, stats: optionStats);

        if (option1 == option2) {
            _levelOption2.Set("ALL OTHER UPGRADES MAXED OUT", "", "");
        } else {
            optionTitle = AvailableProjectileUpgrades[option2];
            optionLevel = UpgradeData[optionTitle].CurrentLevel + 1;
            optionStats = UpgradeData[optionTitle].GetLevelStats(optionLevel);
            // Debug.Log("LevelUpProjectile.RandomizeOptions: option2statsText is " + statsText);   
            _levelOption2.Set(title: optionTitle, description: "Level " + optionLevel, stats: optionStats);
        }
    }

    void SelectUpgrade(int selectedOptionNumber) 
    {
        string selectedUpgrade = "";
        if (selectedOptionNumber == 1) 
        {
            selectedUpgrade = AvailableProjectileUpgrades[option1];
        } else {
            selectedUpgrade = AvailableProjectileUpgrades[option2];
        }
        
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

        Player player = GameObject.Find("Player").GetComponent<Player>();
        if (player != null) 
        {
            switch(selectedUpgrade) 
            {
                case "count++": 
                    player.UpgradeCount(UpgradeData["count++"].CurrentLevel);
                    break;
                case "speed++":
                    player.UpgradeSpeed(UpgradeData["speed++"].CurrentLevel);
                    break;
                case "damage++":
                    player.UpgradeDamage(UpgradeData["damage++"].CurrentLevel);
                    break;
                case "raven":
                    player.UpgradeRaven(UpgradeData["raven"].CurrentLevel);
                    break;
                case "starfish":
                    player.UpgradeStarfish(UpgradeData["starfish"].CurrentLevel);
                    break;
                default: break;
            }
        }

        // resume the game and exit the level up popup
        Time.timeScale = 1; 
        Destroy(gameObject);
        
    }
}
