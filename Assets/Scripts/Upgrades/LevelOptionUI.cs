using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelOptionUI : MonoBehaviour
{
    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private TextMeshProUGUI _stats;
    private Image _icon;

    void Awake()
    {
        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        _stats = transform.Find("Stats").GetComponent<TextMeshProUGUI>();
        _icon = transform.Find("Border/Icon").GetComponent<Image>();
    }

    public void SetMaxedOut()
    {
        _title.text = "ALL OTHER UPGRADES MAXED OUT";
        _description.text = "";
        _stats.text = "";
    }

    // fyi: title -> upgrade name, description -> level, stats -> description
    public void Set(string title, string description, string stats, Sprite icon)
    {
        _title.text = title;
        _description.text = description;
        _stats.text = stats;
        _icon.sprite = icon;
    }
}
