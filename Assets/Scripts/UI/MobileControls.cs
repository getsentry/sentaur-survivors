using UnityEngine;

namespace UI
{
    public class MobileControls : MonoBehaviour
    {
        [SerializeField] private bool _forceEnable;
        private void Awake()
        {
            if (_forceEnable)
            {
                return;
            }
            
            if (Application.platform != RuntimePlatform.Android && 
                Application.platform != RuntimePlatform.IPhonePlayer)
            {
                gameObject.SetActive(false);    
            }
        }
    }
}
