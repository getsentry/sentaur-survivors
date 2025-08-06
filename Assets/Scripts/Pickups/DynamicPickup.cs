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
        _spriteRenderer.material.shader = Shader.Find("Resources/Default.shader");

        if (_enableDynamicShader)
        {
            StartCoroutine(LoadMaterialFromBundle());    
        }
    }
    
    private IEnumerator LoadMaterialFromBundle()
    {
        using var www = UnityWebRequestAssetBundle.GetAssetBundle(_assetBundleUrl);
        www.timeout = 15;
     
        Debug.Log("Dynamic shader enabled! Loading...");
        
        yield return www.SendWebRequest();

        Debug.Log("Success! Applying dynamic shader.");
        
        var bundle = DownloadHandlerAssetBundle.GetContent(www);
        var bundledShader = bundle.LoadAsset<Shader>(_shaderName);
        _spriteRenderer.material.shader = bundledShader;
        bundle.Unload(false);
    }
}
