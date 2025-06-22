using UnityEngine;
using UnityEngine.UI;
// This was used when the SoundManager wasn't a singleton
public class VFXListener : MonoBehaviour
{
    private SoundManager soundManager;
    private Button button;
    
    void OnEnable()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene.");
        }
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found on this GameObject.");
            return;
        }
        button.onClick.AddListener(() =>
        {
            if (soundManager != null)
            {
                soundManager.ToggleSound();
            }
        });
    }
}
