using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;

    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update() { }

    // 0.0f = 0% health, 1.0f = 100% health
    public void SetHealth(float health)
    {
        Debug.Log("HealthBar.SetHealth: Setting health to " + health);
        _slider.value = health;
    }
}
