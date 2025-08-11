using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/DemoConfig.asset", menuName = "DemoConfig", order = 999)]
public class DemoConfiguration : ScriptableObject
{
    [Header("Master Switch")]
    [SerializeField] private bool _enabled;
    
    [Header("Leaderboard Configuration")]
    [SerializeField] private string _apiUrl = string.Empty;
    [SerializeField] private User _user;

    [Header("Demo Settings")]
    [SerializeField] private bool _autoPlay;
    [SerializeField] private bool _notHotDogParticleEffect;
    [SerializeField] private bool _fetchUpgradeFromServer;
    [SerializeField] private bool _crashOnGameOver;
    
    public bool Enabled => _enabled;
    public string ApiUrl => _apiUrl;
    public User User => _user;
    
    public bool AutoPlay => _enabled && _autoPlay;
    public bool NotHotDogParticleEffect => _enabled && _notHotDogParticleEffect;
    public bool FetchUpgradeFromServer => _enabled && _fetchUpgradeFromServer;
    public bool CrashOnGameOver => _enabled && _crashOnGameOver;
}

[Serializable]
public class User
{
    public string Username;
    public string Password;
}