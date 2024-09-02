using System.Collections.Generic;
using Sentry;
using UnityEngine;

class UpgradeManager : MonoBehaviour
{
    List<UpgradePathBase> _availableUpgrades = new List<UpgradePathBase>();

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
        _availableUpgrades.AddRange(GetComponentsInChildren<UpgradePathBase>());

        // dart starts at level 1
        GetComponentInChildren<DartUpgradePath>()
            .LevelUp();
    }

    /**
      * Returns a tuple of random upgrade paths that are valid
      */
    public (UpgradePathBase, UpgradePathBase) GetRandomUpgradePaths()
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

    public void LevelUpUpgradePath(UpgradePathBase upgradePath)
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
