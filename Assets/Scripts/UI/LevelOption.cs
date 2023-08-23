using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class LevelOption : MonoBehaviour
{
    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private TextMeshProUGUI _stats;
     
    [SerializeField]
    private SpriteRenderer _icon;

    void Awake() {
        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        _stats = transform.Find("Stats").GetComponent<TextMeshProUGUI>();
        _icon = transform.Find("Border/Icon").GetComponent<SpriteRenderer>();
    }

    // fyi: title -> upgrade name, description -> level, stats -> description 
    public void Set(string title, string description, string stats, Sprite sprite) {
        _title.text = title;
        _description.text = description;
        _stats.text = stats;
        _icon.sprite = sprite;
    }
}
