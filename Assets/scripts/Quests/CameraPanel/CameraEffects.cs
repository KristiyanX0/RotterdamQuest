using UnityEngine;
using UnityEngine.UI;

public class CameraEffects : MonoBehaviour
{
    
    [SerializeField] private Image overlayImage;     // The effect overlay (transparent PNG)
    [SerializeField] private bool effectEnabledByDefault = false;
    
    private bool isEffectEnabled;
    
    void Start()
    {
        if (overlayImage != null)
        {
            overlayImage.gameObject.SetActive(effectEnabledByDefault);
            isEffectEnabled = effectEnabledByDefault;
        }
    }
    
    public void ToggleEffect()
    {
        isEffectEnabled = !isEffectEnabled;
        if (overlayImage != null)
        {
            overlayImage.gameObject.SetActive(isEffectEnabled);
        }
    }
    
    public void EnableEffect(bool enable)
    {
        isEffectEnabled = enable;
        if (overlayImage != null)
        {
            overlayImage.gameObject.SetActive(enable);
        }
    }
    
    public bool IsEffectEnabled()
    {
        return isEffectEnabled;
    }
}