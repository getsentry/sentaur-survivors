using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Describes a weapon upgrade path, with text description for each level
 * (e.g. Dart levels 1-3)
 */

public class UpgradePath
{
    private string _name;
    private int _currentLevel;
    private List<string> _levelDesc;

    public UpgradePath(List<string> levelStats, int currentLevel = 0)
    {
        _levelDesc = levelStats;
        _currentLevel = currentLevel;
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
            return _levelDesc[level];
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.Log(e.Message);
            return "";
        }
    }
}
