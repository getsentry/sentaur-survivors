using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelOptionUI : MonoBehaviour
{
    /*
    [SerializeField]
    [Tooltip("Sprite for the count++ upgrade")]
    private Sprite _countSprite;

    [SerializeField]
    [Tooltip("Sprite for the speed++ upgrade")]
    private Sprite _speedSprite;

    [SerializeField]
    [Tooltip("Sprite for the damage++ upgrade")]
    private Sprite _damageSprite;

    [SerializeField]
    [Tooltip("Sprite for the dart upgrade")]
    private Sprite _dartSprite;

    [SerializeField]
    [Tooltip("Sprite for the raven upgrade")]
    private Sprite _ravenSprite;

    [SerializeField]
    [Tooltip("Sprite for the starfish upgrade")]
    private Sprite _starfishSprite;

    [SerializeField]
    [Tooltip("Sprite for the schnitzel upgrade")]
    private Sprite _schnitzelSprite;*/

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
        /*
                switch (upgradeType)
                {
                    case UpgradeType.CountUp:
                        _title.text = "count++";
                        _icon.sprite = _countSprite;
                        break;
                    case UpgradeType.CooldownDown:
                        _title.text = "cooldown++"; // should be minus minus?
                        _icon.sprite = _speedSprite;
                        break;
                    case UpgradeType.DamageUp:
                        _title.text = "damage++";
                        _icon.sprite = _damageSprite;
                        break;
                    case UpgradeType.Dart:
                        _title.text = "dart";
                        _icon.sprite = _dartSprite;
                        break;
                    case UpgradeType.Raven:
                        _title.text = "raven";
                        _icon.sprite = _ravenSprite;
                        break;
                    case UpgradeType.Starfish:
                        _title.text = "starfish";
                        _icon.sprite = _starfishSprite;
                        break;
                    case UpgradeType.Schnitzel:
                        _title.text = "schnitzel";
                        _icon.sprite = _schnitzelSprite;
                        break;
                    default:
                        break;
                }*/
    }
}
