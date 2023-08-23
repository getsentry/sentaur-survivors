using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpProjectile : MonoBehaviour
{

    // fyi: title -> upgrade name, description -> level, stats -> description 

    // ????
    // todo - make this a dictionary of string to string, where the key is the upgrade name and the value is the description
    // todo - make this a dictionary of string to string, where the key is the upgrade name and the value is the stats
    // todo - make this a dictionary of string to string, where the key is the upgrade name and the value is the sprite
    // todo - make this a dictionary of string to string, where the key is the upgrade name and the value is the level

    // upgrade[(string) upgradeName] = [description, stats, sprite, level]
    // upgradeObject = upgrade[(string) upgradeName]
    // upgradeObject.description
    // upgradeObject.stats
    // upgradeObject.sprite
    // upgradeObject.level

    // leveling up an upgrade, changes the stats to new level, increases the level #


    public static List<string> AvailableProjectileUpgrades = new List<string>{
        "count++", "speed++", "damage++", "raven", "starfish"
    };

    public static Dictionary<string, int> UpgradesToLevelsMap = new Dictionary<string, int>{
        {"count++", 0}, {"speed++", 0}, {"damage++", 0}, {"raven", 0}, {"starfish", 0}
    };


    //map each AvailableProjectileUpgrades to the stats of the each upgrade
    public static Dictionary<string, string> AvailableProjectileUpgradesStats = new Dictionary<string, string>{
        {"count++", "+1 dart"},
        {"speed++", "+50% dart speed"},
        {"damage++", "+100% base damage"},
        {"raven", "targets the nearest enemy"},
        {"starfish", "orbits and damages enemies"}
    };

    // todo match these stats / descriptions with the strings in AvailableProjectileUpgrades

    private string[] _countUpgrades = {
        "+ 2 darts",
        "+ 2 darts",
        "+ 2 darts"
    };

    private string[] _speedUpgrades = {
        "darts auto-fire faster",
        "darts auto-fire EVEN FASTER",
        "darts auto-fire the fastest they've ever fired B)"
    };

    private string[] _damageUpgrades = {
        "increase base damage",
        "increase base damage MORE",
        "increase base damage EVEN MORE"
    };

    private string[] _ravenUpgrades = {
        "gain a Raven, who targets the nearest enemy every 10 seconds",
        "Raven targets more frequently and deals more damage",
        "gain ANOTHER Raven, with both doing even more damage"
    };

    private string[] _starfishUpgrades = {
        "gain a Starfish, which orbits you for 5 seconds every 8 seconds and deals a bit of damage to every enemy it touches",
        "the Starfish orbits for 3 seconds longer and deals more damage",
        "the Starfish orbits faster and deals even more damage"
    };

    private int MAX_LEVEL = 3;

    // these are equivalent to the index of the option in AvailableProjectileUpgrades
    private int option1;
    private int option2;

    // find the descriptions of the upgrades
    private string option1stats;
    private string option2stats;

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
        option1stats = AvailableProjectileUpgrades[option1];
        Debug.Log("LevelUpProjectile.RandomizeOptions: option1stats is " + option1stats);   

        // select a second option that is different from the first option if the number of available
        // projectiles is greater than 1
        do 
        {
            option2 = Random.Range(0, AvailableProjectileUpgrades.Count);
            option2stats = AvailableProjectileUpgrades[option2];
            Debug.Log("LevelUpProjectile.RandomizeOptions: option2stats is " + option2stats);
        } while (AvailableProjectileUpgrades.Count > 1 && option2 == option1);

        var optionText = AvailableProjectileUpgrades[option1];
        var statsText = AvailableProjectileUpgradesStats[option1stats];
        Debug.Log("LevelUpProjectile.RandomizeOptions: option1statsText is " + statsText);   
        _levelOption1.Set(title: optionText, description: "", stats: statsText);
        if (option1 == option2) {
            _levelOption2.Set("ALL OTHER UPGRADES MAXED OUT", "", "");
        } else {
            optionText = AvailableProjectileUpgrades[option2];
            statsText = AvailableProjectileUpgradesStats[option2stats];
            Debug.Log("LevelUpProjectile.RandomizeOptions: option2statsText is " + statsText);   
            _levelOption2.Set(title: optionText, description: "", stats: statsText);
        }
        // TODO: change sprites, description (use _xxxUpgrades)
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
        
        UpgradesToLevelsMap[selectedUpgrade]++; // level up the selected upgrade

        if (UpgradesToLevelsMap[selectedUpgrade] == MAX_LEVEL) 
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
                    player.UpgradeCount(UpgradesToLevelsMap["count++"]);
                    break;
                case "speed++":
                    player.UpgradeSpeed(UpgradesToLevelsMap["speed++"]);
                    break;
                case "damage++":
                    player.UpgradeDamage(UpgradesToLevelsMap["damage++"]);
                    break;
                case "raven":
                    player.UpgradeRaven(UpgradesToLevelsMap["raven"]);
                    break;
                case "starfish":
                    player.UpgradeStarfish(UpgradesToLevelsMap["starfish"]);
                    break;
                default: break;
            }
        }

        // resume the game and exit the level up popup
        Time.timeScale = 1; 
        Destroy(gameObject);
        
    }
}
