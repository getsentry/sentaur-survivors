using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshPro _text;

    private Sequence _animSequence;

    void Awake()
    {
        _text = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update() { }

    public void SetDamage(int damage)
    {
        _text.text = damage.ToString();
    }

    private void Init(int value)
    {
        // set the text
        this.SetDamage(value);

        // make the text jump up
        _animSequence = transform.DOLocalJump(
            transform.localPosition,
            1.0f, // jump power
            1, // num jumps
            0.5f // duration
        );
    }

    private void OnDestroy()
    {
        if (_animSequence != null)
        {
            _animSequence.Kill();
        }
    }

    // usage: _damageTextPrefab.Spawn(transform, position, 10, 2.0f);

    public DamageText Spawn(Transform parent, Vector2 position, int value, float duration = 1.0f)
    {
        // instantiate the damage text prefab
        var damageText = Instantiate(this, parent);
        damageText.transform.localPosition = position;

        damageText.Init(value);

        // destroy the damage text after a delay
        Destroy(damageText.gameObject, duration);

        return damageText;
    }
}
