using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/DemoConfig.asset", menuName = "DemoConfig", order = 999)]
public class DemoConfiguration : ScriptableObject
{
    public bool Enabled = false;
    public string ApiUrl = string.Empty;
    public string Psk = string.Empty;
}