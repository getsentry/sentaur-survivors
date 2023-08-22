using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{

    private Slider _slider;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    // 0.0f = 0% xp, 1.0f = 100% xp
    public void SetXp(float xp)
    {
        _slider.value = xp;
    }
}
