using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    private string _name;
    private int _currentLevel;
    private List<string> _levelStats;
    private Sprite _upgradeSprite;

    public Upgrade(string name, List<string> levelStats, Sprite upgradeSprite = null, int currentLevel = 0)
    {
        _name = name;
        _levelStats = levelStats;
        _upgradeSprite = upgradeSprite;
        _currentLevel = currentLevel;
    }

    public string Name
    {
        get { return _name; }

        set { _name = value; }
    }

    public int CurrentLevel
    {
        get { return _currentLevel; }
    }

    public void LevelUp()
    {
        _currentLevel++;
    }

    public string GetLevelStats(int level)
    {
        level -= 1; // indexing starts at 0;
        try 
        {
            return _levelStats[level];
        } 
        catch (IndexOutOfRangeException e)
        {
            Debug.Log(e.Message);
            return "";
        }

    }

    public Sprite UpgradeSprite
    {
        get { return _upgradeSprite; }
    }
}
