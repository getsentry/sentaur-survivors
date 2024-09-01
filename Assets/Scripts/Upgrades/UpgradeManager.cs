using System.Collections.Generic;
using Sentry;
using UnityEngine;

class UpgradeManager : MonoBehaviour
{
    List<UpgradeBase> _availableUpgrades = new List<UpgradeBase>();

    private static UpgradeManager _instance;

    public static UpgradeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UpgradeManager>();
            }
            return _instance;
        }
    }

    public void Awake()
    {
        _availableUpgrades.AddRange(GetComponentsInChildren<UpgradeBase>());
    }

    /**
      * Returns a tuple of random upgrade paths that are valid
      */
    public (UpgradeBase, UpgradeBase) GetRandomUpgradeChoices()
    {
        int option1 = Random.Range(0, _availableUpgrades.Count);
        int option2;

        // select a second option that is different from the first option if the number of available
        // projectiles is greater than 1
        do
        {
            option2 = Random.Range(0, _availableUpgrades.Count);
        } while (_availableUpgrades.Count > 1 && option2 == option1);

        return (_availableUpgrades[option1], _availableUpgrades[option2]);
    }

    public void UpgradePath(UpgradeBase upgradePath)
    {
        upgradePath.LevelUp(); // level up the selected upgrade

        // _upgradeData[selectedUpgrade].LevelUp(); // level up the selected upgrade

        if (upgradePath.IsMaxLevel())
        {
            // take the upgrade out of the pool if it's maxed out
            for (int i = 0; i < _availableUpgrades.Count; i++)
            {
                if (_availableUpgrades[i] == upgradePath)
                {
                    _availableUpgrades.RemoveAt(i);
                    break;
                }
            }
        }

        SentrySdk.Metrics.Increment(
            "upgrade_selected",
            tags: new Dictionary<string, string> { { "type", upgradePath.Title } }
        );
    }
}
