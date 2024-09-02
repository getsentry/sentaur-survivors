using UnityEngine;

public class UpgradePathBase : MonoBehaviour
{
    private static int MAX_LEVEL = 3;

    [SerializeField]
    protected int _level = 0; // 0 => inactive, 1 => level 1, ...
    public int Level => _level + 1;
    public int NextLevel => Level + 1;

    [SerializeField]
    protected Sprite _icon;
    public Sprite Icon => _icon;

    [SerializeField]
    protected string _title;
    public string Title => _title;

    [SerializeField]
    // NOTE: _descriptions starts at index 0, but level starts at 1
    protected string[] _descriptions;
    public string NextDescription => _descriptions[_level];

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
        return _level == MAX_LEVEL;
    }

    public void LevelUp()
    {
        if (IsMaxLevel())
        {
            return;
        }

        _level++;

        UpgradeToLevel(_level);
    }

    public virtual void UpgradeToLevel(int level)
    {
        // override this method in child classes
    }
}
