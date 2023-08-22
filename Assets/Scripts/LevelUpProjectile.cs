using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpProjectile : MonoBehaviour
{

    public static List<string> AvailableProjectileUpgrades = new List<string>{
        "count", "speed", "damage"
    };

    public static Dictionary<string, int> UpgradesToLevelsMap = new Dictionary<string, int>{
        {"count", 0}, {"speed", 0}, {"damage", 0}
    };

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

        // option1Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = AvailableProjectileUpgrades[option1];
        // if (option1 == option2) 
        // {
        //     option2Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "ALL OTHER UPGRADES MAXED OUT";
        //     option2Button.enabled = false;
        // } 
        // else 
        // {
        //     option2Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = AvailableProjectileUpgrades[option2];
        // }

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
                case "count": 
                    player.UpgradeCount(UpgradesToLevelsMap["count"]);
                    break;
                case "speed":
                    player.UpgradeSpeed(UpgradesToLevelsMap["speed"]);
                    break;
                case "damage":
                    player.UpgradeDamage(UpgradesToLevelsMap["damage"]);
                    break;
                default: break;
            }
        }

        // resume the game and exit the level up popup
        Time.timeScale = 1; 
        Destroy(gameObject);
        
    }
}
