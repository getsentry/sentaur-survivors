using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpProjectile : MonoBehaviour
{

    // private string[] ProjectileUpgradeTypes = {
    //     "count", "speed", "damage"
    // };

    public static List<string> AvailableProjectileUpgrades = new List<string>{
        "count", "speed", "damage"
    };

    public static Dictionary<string, int> UpgradesToLevelsMap = new Dictionary<string, int>{
        {"count", 0}, {"speed", 0}, {"damage", 0}
    };

    private string[] _countUpgrades = {
        "+ 1 dart",
        "+ 1 dart",
        "ultimate: fires darts in all directions for 15 seconds"
    };

    private string[] _speedUpgrades = {
        "darts auto-fire faster",
        "darts auto-fire EVEN FASTER",
        "ultimate: rapid fire darts for 15 seconds"
    };

    private string[] _damageUpgrades = {
        "increase base damage",
        "increase base damage MORE",
        "ultimate: base damage is insanely high for 15 seconds"
    };

    [SerializeField]
    [Tooltip("The option 1 button")]
    private Button option1Button;

    [SerializeField]
    [Tooltip("The option 2 button")]
    private Button option2Button;

    private int MAX_LEVEL = 2; // change to 3 when implementing ults
    private int option1;
    private int option2;

    // Start is called before the first frame update
    void Start()
    {
        // randomize the two options
        option1 = Random.Range(0, AvailableProjectileUpgrades.Count - 1);
        do {
            option2 = Random.Range(0, AvailableProjectileUpgrades.Count - 1);
    } while (option2 != option1);

        option1Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = AvailableProjectileUpgrades[option1];
        option2Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = AvailableProjectileUpgrades[option2];

        // TODO: sprites, description (use _xxxUpgrades)
    }

    public void SelectUpgrade(int selectedOption) 
    {
        string selection = "";
        if (selectedOption == 1) 
        {
            selection = AvailableProjectileUpgrades[option1];
        } else {
            selection = AvailableProjectileUpgrades[option2];
        }
        UpgradesToLevelsMap[selection]++;
        if (UpgradesToLevelsMap[selection] == MAX_LEVEL) 
        {
            // take the upgrade out of the pool
            for (int i = 0; i < AvailableProjectileUpgrades.Count; i++)
            {
                if (AvailableProjectileUpgrades[i] == selection) 
                {
                    AvailableProjectileUpgrades.RemoveAt(i);
                    break;
                }
            }
        }

        Player player = GameObject.Find("Player").GetComponent<Player>();
        if (player != null) 
        {
            switch(selection) 
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
        
    }
}
