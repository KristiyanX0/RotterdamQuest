using UnityEngine;
using UnityEngine.UI;

public class CompassPointer : MonoBehaviour
{
    [SerializeField]
    private RectTransform playerIcon; // Reference to the player icon RectTransform
    public float updateInterval = 1f; // Time interval in seconds to check for heading changes
    public float degreeThreshold = 5f; // Minimum change in degrees required to update the icon
    public float degreeThresholdImidiate = 10f; // Minimum change in degrees for imidiate change of icon

    [SerializeField]
    private float animationDuration = 0.25f; // Duration of rotation in seconds
    private float lastUpdateTime; // Timer to track the last update time
    private float lastHeading; // Last recorded heading

    void Start()
    {
        // Enable compass and gyro if available
        if (!Input.compass.enabled)
        {
            Input.compass.enabled = true;
        }

        if (!Input.gyro.enabled)
        {
            Input.gyro.enabled = true;
        }

        lastUpdateTime = Time.time; // Initialize the last update time
        lastHeading = Input.compass.trueHeading; // Initialize the last heading
    }

    void Update()
    {
        // Check if compass is available and enabled before using it
        if (Input.compass.enabled)
        {
            float currentHeading = Input.compass.trueHeading;
            float headingDifference = Mathf.Abs(currentHeading - lastHeading);

            // Check if the update interval has passed and if the heading change is above the threshold
            // if ((Time.time - lastUpdateTime >= updateInterval && headingDifference >= degreeThreshold) || (headingDifference >= degreeThresholdImidiate))
            if ((Time.time - lastUpdateTime >= updateInterval) || (headingDifference >= degreeThresholdImidiate))
            {
                // Update the player icon rotation
                // playerIcon.localRotation = Quaternion.Euler(0, 0, -currentHeading);
                StopAllCoroutines();
                StartCoroutine(AnimateRottation(-currentHeading));

                // Update the last heading and last update time
                lastHeading = currentHeading;
                lastUpdateTime = Time.time;
            }
        }
        else
        {
            // Debug.LogWarning("Compass is not enabled or available on this device.");
        }
    }

    private System.Collections.IEnumerator AnimateRottation(float targetRotation)
    {
        float startRotation = playerIcon.eulerAngles.z;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            float newRotation = Mathf.LerpAngle(startRotation, targetRotation, elapsedTime / animationDuration);
            playerIcon.rotation = Quaternion.Euler(0, 0, newRotation);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure position and rotation snap to the final values
        playerIcon.rotation = Quaternion.Euler(0, 0, targetRotation);
    }
}
