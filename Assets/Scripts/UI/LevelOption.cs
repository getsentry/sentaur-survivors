using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class LevelOption : MonoBehaviour
{
    private TextMeshProUGUI _title;
    private TextMeshProUGUI _levelLabel;
    private TextMeshProUGUI _stats;
    // private Image _icon;

    void Awake() {
        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _levelLabel = transform.Find("LevelLabel").GetComponent<TextMeshProUGUI>();
        _stats = transform.Find("Stats").GetComponent<TextMeshProUGUI>();
        // _icon = transform.Find("Border/Icon").GetComponent<Image>();
    }

    public void Set(string title, string levelLabel, string stats) {
        _title.text = title;
        _levelLabel.text = levelLabel;
        _stats.text = stats;
    }
}
