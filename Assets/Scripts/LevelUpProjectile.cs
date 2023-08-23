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
        "count++", "cooldown++", "damage++", "dart", "raven", "starfish"
    };

    public static Dictionary<string, Upgrade> UpgradeData = new Dictionary<string, Upgrade>{
        {
            "count++",
            new Upgrade("count++", new List<string>{"+1 projectile", "+1 projectile", "+2 projectiles"})
        },
        {
            "cooldown++",
            new Upgrade("cooldown++", new List<string>{"-33% base cooldown", "-50% base cooldown", "-75% base cooldown"})
        },
        {
            "damage++",
            new Upgrade("damage++", new List<string>{"+30% base damage", "+60% base damage", "+100% base damage"})
        },
        {
            "dart",
            new Upgrade("dart", new List<string>{"gain an additional dart that fires in the opposite direction", "+50% damage", "+100% damage and gain two additional darts firing in the opposite direction"})
        },
        {
            "raven",
            new Upgrade("raven", new List<string>{"auto-targets the nearest enemy after a set cooldown", "gain an additional raven", "+50% damage and -33% cooldown"})
        },
        {
            "starfish",
            new Upgrade("starfish", new List<string>{"auto-orbits you for a set duration after a set cooldown, damaging every enemy you hit", "+50% orbit duration", "+100% orbit duration and -50% cooldown"})
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

        int newLevel = UpgradeData[selectedUpgrade].CurrentLevel;
     
        switch(selectedUpgrade) 
        {
            case "count++": 
                ProjectileBase.UpgradeCount(newLevel);
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
