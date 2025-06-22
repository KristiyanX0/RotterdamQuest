using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour
{
    public GameObject cameraDisplayObject; // The GameObject containing the RawImage
    public GameObject takePhotoButton;

    // public Button toggleCameraButton; // Button to toggle the camera
    public Button startCameraButton; // New button to start the camera
    public Button stopCameraButton; // New button to stop the camera

    [SerializeField]
    private GameObject galleryService;
    private WebCamTexture webcamTexture;
    private RawImage cameraDisplay;
    private RectTransform cameraDisplayRectTransform;
    private bool isCameraActive = false;

    void Start()
    {
        // Assign a listener to the buttons
        // toggleCameraButton.onClick.AddListener(ToggleCamera);
        startCameraButton.onClick.AddListener(StartCamera);
        stopCameraButton.onClick.AddListener(StopCamera);

        // Get the RawImage component
        cameraDisplay = cameraDisplayObject.GetComponent<RawImage>();
        cameraDisplayRectTransform = cameraDisplayObject.GetComponent<RectTransform>();

        // Ensure the camera display is inactive initially
        cameraDisplayObject.SetActive(false);
        takePhotoButton.SetActive(false);
    }

    public void CaptureImage()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            galleryService.GetComponent<GalleryService>().saveImage(webcamTexture);
            
            Debug.LogWarning("Image captured!");
            // Save the captured image as a PNG file
        }
        else
        {
            Debug.LogError("Camera feed is not active!");
        }
    }

    void ToggleCamera()
    {
        if (isCameraActive)
        {
            StopCamera();
        }
        else
        {
            StartCamera();
        }
    }

    void StartCamera()
    {
        if (webcamTexture == null)
        {
            // Initialize the WebCamTexture
            webcamTexture = new WebCamTexture(2400, 1080, 60);
            cameraDisplay.texture = webcamTexture;
        }

        if (!webcamTexture.isPlaying)
        {
            webcamTexture.Play();
            cameraDisplayObject.SetActive(true);
            takePhotoButton.SetActive(true);
            AdjustCameraRotation();
            isCameraActive = true;

            // Adjust the display rotation to match the camera feed
        }
    }

    void StopCamera()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
            cameraDisplayObject.SetActive(false);
            takePhotoButton.SetActive(false);
            isCameraActive = false;
        }
    }


    void AdjustCameraRotation()
    {
        if (webcamTexture != null)
        {
            // Get the rotation angle from the WebCamTexture
            int videoRotationAngle = webcamTexture.videoRotationAngle;

            // Adjust the rotation of the RawImage to match the camera feed
            cameraDisplayRectTransform.rotation = Quaternion.Euler(0, 0, -videoRotationAngle);

            // Flip the image if needed based on WebCamTexture's videoVerticallyMirrored property
            cameraDisplayRectTransform.localScale = new Vector3(
                webcamTexture.videoVerticallyMirrored ? -1 : 1,
                1,
                1
            );
        }
    }

    
}
