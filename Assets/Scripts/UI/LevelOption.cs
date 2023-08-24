using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class LevelOption : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Sprite for the count++ upgrade")]
    private Sprite _countSprite;

    [SerializeField]
    [Tooltip("Sprite for the speed++ upgrade")]
    private Sprite _speedSprite;

    [SerializeField]
    [Tooltip("Sprite for the damage++ upgrade")]
    private Sprite _damageSprite;

    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private TextMeshProUGUI _stats;
    private Image _icon;

    void Awake() {
        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        _stats = transform.Find("Stats").GetComponent<TextMeshProUGUI>();
        _icon = transform.Find("Border/Icon").GetComponent<Image>();
    }

    // fyi: title -> upgrade name, description -> level, stats -> description 
    public void Set(string title, string description, string stats) {
        _title.text = title;
        _description.text = description;
        _stats.text = stats;

        switch(title)
        {
            case "count++": 
                _icon.sprite = _countSprite;
                break;
            case "speed++":
                _icon.sprite = _speedSprite;
                break;
            case "damage++":
                _icon.sprite = _damageSprite;
                break;
            default: break;
        }
    }
}
