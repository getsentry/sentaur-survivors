using System.Collections.Generic;
using UnityEngine;

class UpgradeManager : MonoBehaviour
{
    List<UpgradeBase> _upgrades = new List<UpgradeBase>();

    public void Awake()
    {
        _upgrades.AddRange(GetComponentsInChildren<UpgradeBase>());
    }
}
