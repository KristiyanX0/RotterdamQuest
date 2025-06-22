using UnityEngine;
using UnityEngine.UI;

public class CompassSpriteChanger : MonoBehaviour
{
    public Image compassImage;
    public Sprite[] compassSprites;
    private float[] angleThresholds;

    void Start()
    {
        if (!Input.compass.enabled)
        {
            Input.compass.enabled = true;
        }

        angleThresholds = new float[compassSprites.Length];
        for (int i = 0; i < angleThresholds.Length; i++)
        {
            angleThresholds[i] = 360f / compassSprites.Length * i;
        }
    }

    void Update()
    {
        if (Input.compass.enabled)
        {
            float heading = Input.compass.trueHeading;
            UpdateCompassSprite(heading);
        }
        else
        {
            // Debug.LogWarning("Compass is not enabled or available on this device.");
        }
    }

    void UpdateCompassSprite(float heading)
    {
        // Determine which sprite to use based on the heading
        int spriteIndex = Mathf.FloorToInt((heading / 360f) * compassSprites.Length) % compassSprites.Length;

        // Assign the corresponding sprite
        compassImage.sprite = compassSprites[spriteIndex];
    }
}
