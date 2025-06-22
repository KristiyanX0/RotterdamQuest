using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    [SerializeField]
    private RectTransform objectRect; // Reference to the RectTransform of the compass

    // Positions to hide and show the compass
    public float offscreenPositionX = -300f; // X position to hide the compass
    public float offscreenPositionY = 300f; // Y position to hide the compass
    public float animationDuration = 0.3f; // Time for the animation
    public float rotationAngle = -33f; // Rotation angle when hiding the compass

    public bool isHidden {get; private set; } // Track whether the compass is hidden
    private Vector3 shownPosition;
    private float shownRotationAngle;
    private Vector3 hidenPosition;

    void Start()
    {
        if (objectRect == null)
        {
            objectRect = GetComponent<RectTransform>(); // Auto-get RectTransform if not set
        }

        shownPosition = objectRect.anchoredPosition;
        shownRotationAngle = objectRect.rotation.eulerAngles.z;
        hidenPosition = new Vector3(offscreenPositionX, offscreenPositionY, 0);
        isHidden = true;
        SetPosition(isHidden);
    }

    public void Toggle()
    {
        if (isHidden)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Hide()
    {
        // Move the compass to the offscreen X and Y positions and set rotation
        StopAllCoroutines();
        StartCoroutine(AnimateToggle(new Vector3(hidenPosition.x, hidenPosition.y, 0), rotationAngle));
        isHidden = true;
    }

    public void Show()
    {
        // Move the compass back to the original X and Y positions and reset rotation
        StopAllCoroutines();
        StartCoroutine(AnimateToggle(new Vector3(shownPosition.x, shownPosition.y, 0), shownRotationAngle));
        isHidden = false;
    }

    public void ShowHide() {
        StartCoroutine(AnimateToggle(new Vector3(shownPosition.x, shownPosition.y, 0), shownRotationAngle, () =>
        {
            StartCoroutine(AnimateToggle(new Vector3(hidenPosition.x, hidenPosition.y, 0), rotationAngle));
        }));
        isHidden = true;
    }

    public void SetPosition(bool isHidden)
    {
        if (isHidden)
        {
            objectRect.anchoredPosition = new Vector3(hidenPosition.x, hidenPosition.y, 0);
            objectRect.eulerAngles = new Vector3(0, 0, rotationAngle);
        }
        else
        {
            objectRect.anchoredPosition = new Vector3(shownPosition.x, shownPosition.y, 0);
            objectRect.eulerAngles = new Vector3(0, 0, shownRotationAngle);;
        }
    }

    private System.Collections.IEnumerator AnimateToggle(Vector3 targetPosition, float targetRotation, System.Action callback = null)
    {
        Vector3 startPosition = objectRect.anchoredPosition;
        float startRotation = objectRect.eulerAngles.z;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            // Lerp position
            objectRect.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);

            // Lerp rotation
            float newRotation = Mathf.LerpAngle(startRotation, targetRotation, elapsedTime / animationDuration);
            objectRect.rotation = Quaternion.Euler(0, 0, newRotation);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure position and rotation snap to the final values
        objectRect.anchoredPosition = targetPosition;
        objectRect.rotation = Quaternion.Euler(0, 0, targetRotation);

        callback?.Invoke();
    }
}

