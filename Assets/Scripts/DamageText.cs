using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DamageText : MonoBehaviour
{
    private TextMeshPro _text;
    void Awake()
    {
        _text = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDamage(int damage)
    {
        _text.text = damage.ToString();
    }
}
