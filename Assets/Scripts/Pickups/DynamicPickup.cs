using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class DynamicPickup : PickupBase
{
    [SerializeField] private string _assetBundleUrl = "https://aspnetcore.empower-plant.com/bundles/special-shaders-v2.bundle";
    [SerializeField] private string _shaderName = "SpecialPickupShader";
    [SerializeField] private bool _enableDynamicShader = true;
    
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_enableDynamicShader)
        {
            Debug.Log("Dynamic shader enabled! Loading...");
            StartCoroutine(LoadMaterialFromBundle());    
        }
    }
    
    private IEnumerator LoadMaterialFromBundle()
    {
        using var www = UnityWebRequestAssetBundle.GetAssetBundle(_assetBundleUrl);
        www.timeout = 15;
            
        yield return www.SendWebRequest();

        Debug.Log("Success! Applying dynamic shader.");
        
        _spriteRenderer.material.shader = null; // Reset
            
        var bundle = DownloadHandlerAssetBundle.GetContent(www);
        var bundledShader = bundle.LoadAsset<Shader>(_shaderName);
        _spriteRenderer.material.shader = bundledShader;
        bundle.Unload(false);
    }
}
