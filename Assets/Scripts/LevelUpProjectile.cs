using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpProjectile : MonoBehaviour
{

    public static List<string> AvailableProjectileUpgrades = new List<string>{
        "Moar Darts!", "Speed Up!", "Damage Up!", "Unleash Raven!"
    };

    public static Dictionary<string, int> UpgradesToLevelsMap = new Dictionary<string, int>{
        {"Moar Darts!", 0}, {"Speed Up!", 0}, {"Damage Up!", 0}, {"Unleash Raven!", 0}
    };

    private string[] _countUpgrades = {
        "+ 2 darts",
        "+ 2 darts",
        "+ 2 darts"
    };

    //map each AvailableProjectileUpgrades to the stats of the _countUpgrades
    public static Dictionary<string, string> AvailableProjectileUpgradesStats = new Dictionary<string, string>{
        {"Moar Darts!", "+1 dart"},
        {"Speed Up!", "+50% dart speed"},
        {"Damage Up!", "+100% base damage"},
        {"Unleash Raven!", "Raven targets the nearest enemy"}
    };

    private string[] _speedUpgrades = {
        "+50% dart speed",
        "+100% dart speed",
        "+150% dart speed"
    };

    private string[] _damageUpgrades = {
        "+100% base damage",
        "+200% base damage",
        "+300% base damage"
    };

    private string[] _ravenUpgrades = {
        "Raven targets the nearest enemy",
        "Raven damage+ and Raven speed up",
        "TWO Raven, even more damage!"
    };

    private int MAX_LEVEL = 3;

    // these are equivalent to the index of the option in AvailableProjectileUpgrades
    private int option1;
    private int option2;

    // find the descriptions of the upgrades
    private string option1stats;
    private string option2stats;

    // find the level of the upgrades
    private string option1level;
    private string option2level;

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
        option1level = UpgradesToLevelsMap[option1stats].ToString();
        Debug.Log("LevelUpProjectile.RandomizeOptions: option1 is " + option1);
        Debug.Log("LevelUpProjectile.RandomizeOptions: option1level is " + option1level);
        Debug.Log("LevelUpProjectile.RandomizeOptions: option1description is " + option1stats);    

        // select a second option that is different from the first option if the number of available
        // projectiles is greater than 1
        do 
        {
            option2 = Random.Range(0, AvailableProjectileUpgrades.Count);
            option2stats = AvailableProjectileUpgrades[option2];
            option2level = UpgradesToLevelsMap[option2stats].ToString();
            Debug.Log("LevelUpProjectile.RandomizeOptions: option2 is " + option2);
            Debug.Log("LevelUpProjectile.RandomizeOptions: option2level is " + option2level);
            Debug.Log("LevelUpProjectile.RandomizeOptions: option2description is " + option2stats);
        } while (AvailableProjectileUpgrades.Count > 1 && option2 == option1);

        var optionText = AvailableProjectileUpgrades[option1];
        var descriptionText = AvailableProjectileUpgradesStats[option1stats];
        _levelOption1.Set(title: optionText, levelLabel: "Level 1", stats: descriptionText);
        if (option1 == option2) {
            _levelOption2.Set("ALL OTHER UPGRADES MAXED OUT", "", "");
        } else {
            optionText = AvailableProjectileUpgrades[option2];
            descriptionText = AvailableProjectileUpgradesStats[option2stats];
            _levelOption2.Set(title: optionText, levelLabel: "Level 1", stats: descriptionText);
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
                case "Moar Darts!": 
                    player.UpgradeCount(UpgradesToLevelsMap["Moar Darts!"]);
                    break;
                case "Speed Up!":
                    player.UpgradeSpeed(UpgradesToLevelsMap["Speed Up!"]);
                    break;
                case "Damage Up!":
                    player.UpgradeDamage(UpgradesToLevelsMap["Damage Up!"]);
                    break;
                case "Unleash Raven!":
                    player.UpgradeRaven(UpgradesToLevelsMap["Unleash Raven!"]);
                    break;
                default: break;
            }
        }

        // resume the game and exit the level up popup
        Time.timeScale = 1; 
        Destroy(gameObject);
        
    }
}
