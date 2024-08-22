using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/DemoConfig.asset", menuName = "DemoConfig", order = 999)]
public class DemoConfiguration : ScriptableObject
{
    public bool Enabled = false;
    public string ApiUrl = string.Empty;
    public User User;
}

[Serializable]
public class User
{
    public string Username;
    public string Password;
}