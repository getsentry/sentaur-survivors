using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/DemoConfig.asset", menuName = "DemoConfig", order = 999)]
public class DemoConfiguration : ScriptableObject
{
    public bool Enabled = false;
    public string ApiUrl = string.Empty;
    public User User;

    public bool AutoPlay;
    public bool CheatingButtons;
    public bool NotHotDogParticleEffect;
    public bool FetchUpgradeFromServer;
    public bool CrashOnGameOver;
}

[Serializable]
public class User
{
    public string Username;
    public string Password;
}