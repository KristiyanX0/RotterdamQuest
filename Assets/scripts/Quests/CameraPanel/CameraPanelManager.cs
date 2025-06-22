using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Events; // Added for List<GameObject>

public class CameraPanelManager : MonoBehaviour
{
    public GameObject cameraDisplayObject; // The GameObject containing the RawImage
    [SerializeField]
    private GameObject galleryService;
    [SerializeField]
    private CameraEffects cameraEffects; // Reference to our effects manager
    [SerializeField]
    private bool applyEffectWhenCapturing = true; // Whether to apply effect when capturing
    [SerializeField] // Add this field to specify UI elements to hide
    private List<GameObject> elementsToHideOnCapture = new List<GameObject>();
    
    private RawImage cameraDisplay; // UI element to display the camera feed
    private WebCamTexture webCamTexture; // Texture to display camera data
    private RectTransform cameraDisplayRectTransform;
    public UnityEvent onCaptureImage; // UnityEvent to trigger after capturing image

    void Start()
    {
        cameraDisplay = cameraDisplayObject.GetComponent<RawImage>();
        cameraDisplayRectTransform = cameraDisplayObject.GetComponent<RectTransform>();
        StartCamera();
    }

    public void StartCamera()
    {
        webCamTexture = new WebCamTexture(2400, 1080, 60);
        cameraDisplay.texture = webCamTexture;
        webCamTexture.Play();
        AdjustCameraRotation();
        Debug.Log("Camera started.");
    }

    public void CaptureImage()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {   
            // Turn on effect if it should be applied when capturing
            bool effectWasEnabled = false;
            if (cameraEffects != null && applyEffectWhenCapturing)
            {
                effectWasEnabled = cameraEffects.IsEffectEnabled();
                cameraEffects.EnableEffect(true);
            }
            
            // Small delay to ensure the effect is visible in the capture
            StartCoroutine(CaptureAfterDelay(0.1f, effectWasEnabled, elementsToHideOnCapture)); // Pass the list here
        }
        else
        {
            Debug.LogError("Camera feed is not active!");
        }
    }

    public void CaptureImageCallback()
    {
        CaptureImage();
    }
    
    private System.Collections.IEnumerator CaptureAfterDelay(float delay, bool previousEffectState, List<GameObject> elementsToHide)
    {
        // Store the original active states of the elements to hide
        Dictionary<GameObject, bool> originalActiveStates = new Dictionary<GameObject, bool>();
        if (elementsToHide != null)
        {
            foreach (var element in elementsToHide)
            {
                if (element != null)
                {
                    originalActiveStates[element] = element.activeSelf;
                    element.SetActive(false);
                }
            }
        }

        // Wait for effect to be visible if one is being applied
        if (cameraEffects != null && applyEffectWhenCapturing && cameraEffects.IsEffectEnabled() != true) {
             yield return new WaitForSeconds(delay);
        }

        // Wait for the end of the frame to ensure all UI and camera view is rendered
        yield return new WaitForEndOfFrame();

        // Create a texture to hold the screenshot
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // Read the pixels from the screen
        // Note: If your game view is not full screen, you might need to adjust the Rect
        // to capture only the relevant part of the screen.
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply(); // Apply changes to the texture

        // Get the GalleryService component
        var galleryComponent = galleryService.GetComponent<GalleryService>();
        if (galleryComponent == null)
        {
            // This line might be problematic if galleryService is not already an initialized GameObject
            // Consider ensuring galleryService is a prefab or properly set up if it can be null.
            galleryComponent = galleryService.AddComponent<GalleryService>();
        }
        
        // Save the combined screenshot (camera feed + UI)
        galleryComponent.saveImage(screenShot);
        
        // Restore previous effect state if needed
        if (cameraEffects != null && applyEffectWhenCapturing)
        {
            cameraEffects.EnableEffect(previousEffectState);
        }

        // Restore the original active states of the hidden elements
        if (elementsToHide != null)
        {
            foreach (var element in elementsToHide)
            {
                if (element != null && originalActiveStates.ContainsKey(element))
                {
                    element.SetActive(originalActiveStates[element]);
                }
            }
        }


        onCaptureImage.Invoke(); // Trigger the UnityEvent after capturing the image
    }

    public void StopCamera()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
            Debug.Log("Camera stopped.");
        }
    }

    public void ToggleEffect()
    {
        if (cameraEffects != null)
        {
            cameraEffects.ToggleEffect();
        }
    }

    void AdjustCameraRotation()
    {
        if (webCamTexture != null)
        {
            // Get the rotation angle from the WebCamTexture
            int videoRotationAngle = webCamTexture.videoRotationAngle;

            // Adjust the rotation of the RawImage to match the camera feed
            cameraDisplayRectTransform.rotation = Quaternion.Euler(0, 0, -videoRotationAngle);

            // Flip the image if needed based on WebCamTexture's videoVerticallyMirrored property
            cameraDisplayRectTransform.localScale = new Vector3(
                webCamTexture.videoVerticallyMirrored ? -1 : 1,
                1,
                1
            );
        }
    }
}
