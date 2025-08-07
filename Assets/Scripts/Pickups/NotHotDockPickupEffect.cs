using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NotHotDockPickupEffect : MonoBehaviour
{
    [SerializeField] private string _assetBundleUrl = "https://aspnetcore.empower-plant.com/bundles/special-shaders-v2.bundle";
    [SerializeField] private string _shaderName = "SpecialShaderEffect";
    [SerializeField] private float _lifeTime = 2;
    
    private ParticleSystemRenderer _renderer;
    
    private void Awake()
    {
        _renderer = GetComponent<ParticleSystemRenderer>();
        _renderer.material.shader = Shader.Find("Resources/Default.shader");
        
        StartCoroutine(DestroyYourself());
        StartCoroutine(LoadMaterialFromBundle());
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
        _renderer.material.shader = bundledShader;
        bundle.Unload(false);
    }
    
    private IEnumerator DestroyYourself()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(this);
    }
}
