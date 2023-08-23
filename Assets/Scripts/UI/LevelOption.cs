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
    // private Image _icon;

    void Awake() {
        _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        _stats = transform.Find("Stats").GetComponent<TextMeshProUGUI>();
        // _icon = transform.Find("Border/Icon").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _description.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // fyi: title -> upgrade name, description -> level, stats -> description 
    public void Set(string title, string description, string stats) {
        _title.text = title;
        // _description.text = description;
        _stats.text = stats;
    }
}
