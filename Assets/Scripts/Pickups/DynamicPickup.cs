using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class DynamicPickup : PickupBase
{
    [SerializeField] private string _assetBundleUrl = "https://cdn.gameserver.com/bundles/special-shaders-v2.bundle";
    [SerializeField] private string _shaderName = "SpecialPickupShader";
    [SerializeField] private bool _enableDynamicShader = true;
    
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_enableDynamicShader)
        {
            StartCoroutine(LoadMaterialFromBundle());    
        }
    }
    
    private IEnumerator LoadMaterialFromBundle()
    {
        using var www = UnityWebRequestAssetBundle.GetAssetBundle(_assetBundleUrl);
        www.timeout = 15;
            
        yield return www.SendWebRequest();
            
        if (www.result == UnityWebRequest.Result.Success)
        {
            var bundle = DownloadHandlerAssetBundle.GetContent(www);
            var bundledShader = bundle.LoadAsset<Shader>(_shaderName);
            _spriteRenderer.material.shader = bundledShader;
        }
        else
        {
            Debug.LogWarning($"Failed to download asset bundle from {_assetBundleUrl}: {www.error}. Using fallback.");
            _spriteRenderer.material.shader = Shader.Find("Resources/DynamicFallback.shader");
        }
    }
}
