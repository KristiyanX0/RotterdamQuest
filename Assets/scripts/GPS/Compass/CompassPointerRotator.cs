using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class CompassPointerRotator : MonoBehaviour
{
    public RectTransform pointerTransform;
    private float heading = 0f;
    private const float LERP_TIME = 2.5f;
    private const float HEADING_THRESHOLD = 5f;

    void Start()
    {
        if (!Input.compass.enabled)
        {
            Input.compass.enabled = true;
        }
    }

    void Update()
    {
        if (Input.compass.enabled)
        {
            float targetHeading = Input.compass.trueHeading;

            if (Mathf.Abs(targetHeading - heading) >= HEADING_THRESHOLD)
            {
                heading = Mathf.LerpAngle(heading, targetHeading, Time.deltaTime * LERP_TIME);
                pointerTransform.localEulerAngles = new Vector3(0, 0, heading);
            }
        }
    }
}
