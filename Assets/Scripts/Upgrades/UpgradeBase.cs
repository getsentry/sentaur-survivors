using UnityEngine;

public class UpgradeBase : MonoBehaviour
{
    private static int MAX_LEVEL = 3;

    [SerializeField]
    protected int _currentLevel;

    [SerializeField]
    private Sprite _icon;

    [SerializeField]
    private string _title;

    [SerializeField]
    private string[] _descriptions;

    private void Awake()
    {
        // assert _descriptions is not higher than max level
        if (_descriptions.Length != MAX_LEVEL)
        {
            Debug.LogError("UpgradeBase: _descriptions length is not equal to MAX_LEVEL");
        }
    }

    public bool IsMaxLevel()
    {
        return _currentLevel == MAX_LEVEL;
    }

    public void LevelUp()
    {
        if (IsMaxLevel())
        {
            return;
        }

        _currentLevel++;

        UpgradeToLevel(_currentLevel);
    }

    public virtual void UpgradeToLevel(int level)
    {
        // override this method in child classes
    }
}
