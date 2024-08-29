using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UpgradeEventData
{
    public UpgradeType UpgradeType;

    public int Level;

    public UpgradeEventData(UpgradeType upgradeType, int level)
    {
        this.UpgradeType = upgradeType;
        this.Level = level;
    }
}
