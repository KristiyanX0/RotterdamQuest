using UnityEngine;

public class CompassToggle : MonoBehaviour
{
    [SerializeField]
    private RectTransform compassRect;
    public float offscreenPositionX = -500f;
    public float offscreenPositionY = -950f; 
    public float animationDuration = 0.3f;
    public float rotationAngle = 90f;

    private bool isHidden;
    private Vector3 shownPosition;
    private Vector3 hidenPosition;

    void Start()
    {
        if (compassRect == null)
        {
            compassRect = GetComponent<RectTransform>();
        }

        shownPosition = compassRect.anchoredPosition;
        hidenPosition = new Vector3(offscreenPositionX, offscreenPositionY, 0);
        isHidden = true;
        SetCompassPosition(isHidden);
    }

    public void ToggleCompass()
    {
        if (isHidden)
        {
            ShowCompass();
        }
        else
        {
            HideCompass();
        }
    }

    private void HideCompass()
    {
        // Move the compass to the offscreen X and Y positions and set rotation
        StopAllCoroutines();
        StartCoroutine(AnimateCompassToggle(new Vector3(hidenPosition.x, hidenPosition.y, 0), rotationAngle));
        isHidden = true;
    }

    private void ShowCompass()
    {
        // Move the compass back to the original X and Y positions and reset rotation
        StopAllCoroutines();
        StartCoroutine(AnimateCompassToggle(new Vector3(shownPosition.x, shownPosition.y, 0), 0f));
        isHidden = false;
    }

    public void SetCompassPosition(bool isHidden)
    {
        if (isHidden)
        {
            compassRect.anchoredPosition = new Vector3(hidenPosition.x, hidenPosition.y, 0);
            compassRect.eulerAngles = new Vector3(0, 0, rotationAngle);
        }
        else
        {
            compassRect.anchoredPosition = new Vector3(shownPosition.x, shownPosition.y, 0);
            compassRect.eulerAngles = Vector3.zero;
        }
    }

    private System.Collections.IEnumerator AnimateCompassToggle(Vector3 targetPosition, float targetRotation)
    {
        Vector3 startPosition = compassRect.anchoredPosition;
        float startRotation = compassRect.eulerAngles.z;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            compassRect.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);

            float newRotation = Mathf.LerpAngle(startRotation, targetRotation, elapsedTime / animationDuration);
            compassRect.rotation = Quaternion.Euler(0, 0, newRotation);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure position and rotation snap to the final values
        compassRect.anchoredPosition = targetPosition;
        compassRect.rotation = Quaternion.Euler(0, 0, targetRotation);
    }
}

