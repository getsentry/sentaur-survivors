using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int BaseDamage = 10;

    public float BaseCooldownPercentage = 1f;

    public int BaseCount = 1;

    private void Start()
    {
        BaseDamage = 10;
        BaseCooldownPercentage = 1f;
        BaseCount = 1;
    }

    private void Update()
    {
        // Add weapon behavior code here
    }

    // Add other methods and functions for weapon functionality
}
