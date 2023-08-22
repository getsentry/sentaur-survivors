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

    // usage: _damageTextPrefab.Spawn(transform, position, 10, 2.0f);

    public DamageText Spawn(Transform parent, Vector2 position, int value, float duration = 2.0f) {
        // instantiate the damage text prefab
        var damageText = Instantiate(this, parent);
        damageText.transform.localPosition = position;

        // set the text
        damageText.SetDamage(value);

        // destroy the damage text after a delay
        Destroy(damageText.gameObject, duration);

        return damageText;
    }
}
