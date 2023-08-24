using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;


public class PlayerText : MonoBehaviour
{
    private TextMeshPro _text;
    void Awake()
    {
        _text = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    private void Init(string text) {

        // set the text
        this.SetText(text);

        // make the text jump up
        transform.DOLocalJump(transform.localPosition, 
            1.5f, // jump power
            1,    // num jumps
            1.5f  // duration
        );
    }

    public PlayerText Spawn(Transform parent, Vector2 position, string text, float duration = 1.0f) {
        // instantiate the damage text prefab
        var playerText = Instantiate(this, parent);
        playerText.transform.localPosition = position;

        playerText.Init(text);

        // destroy the damage text after a delay
        Destroy(playerText.gameObject, duration);

        return playerText;
    }
}
