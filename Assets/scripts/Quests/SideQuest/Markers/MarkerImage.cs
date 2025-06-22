using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
// NOT WORKING YET
public class MarkerImage : MonoBehaviour
{
    private Image markerImage; // Assign this in the Inspector
    public List<Sprite> markerSprites; // Assign your different marker sprites in the Inspector
    public Sprite openedMarkerSprite;
    public bool opened = false;
    private DynamicMapController dynamicMapController; // Assign your DynamicMapController in the Inspector

    public float minZoom;
    public float maxZoom;
    public float currentZoom;

    void Start()
    {
        if (markerImage == null)
        {
            markerImage = GetComponent<Image>();
        }

        if (dynamicMapController == null)
        {
            dynamicMapController = FindObjectOfType<DynamicMapController>();
            if (dynamicMapController == null)
            {
                Debug.LogError("[MarkerImage] DynamicMapController could not be found in the scene.");
            }
        }
        else if (openedMarkerSprite == null)
        {
            Debug.LogWarning("[MarkerImage] openedMarkerSprite is not assigned. Using default sprite.");
        }
        else
        {
            Debug.LogError("[MarkerImage] DynamicMapController not assigned to MarkerImage script.");
            minZoom = 0.5f;
            maxZoom = 2.0f;
            currentZoom = 1.0f;
        }

        // Initialize with the first sprite or a default one
        if (markerSprites != null && markerSprites.Count > 0)
        {
            markerImage.sprite = markerSprites[0];
        }

        GetComponent<Button>().onClick.AddListener(Toggle);
        opened = false;
    }

    public void close()
    {
        opened = false;
        GetComponent<RectTransform>().sizeDelta = new Vector2(146, 106);
        markerImage.sprite = markerSprites[0];
    }

    public void open()
    {
        opened = true;
        GetComponent<RectTransform>().sizeDelta = new Vector2(568, 106);
        markerImage.sprite = openedMarkerSprite;
    }

    public void Toggle()
    {
        if (opened)
        {
            close();
        }
        else
        {
            open();
        }

    }

    void Update()
    {
        if (dynamicMapController != null && markerSprites != null && markerSprites.Count > 0 && !opened)
        {
            UpdateMarkerSpriteBasedOnZoom();
        }
    }

    private void UpdateMarkerSpriteBasedOnZoom()
    {
        minZoom = dynamicMapController.minZoom;
        maxZoom = dynamicMapController.maxZoom;
        currentZoom = dynamicMapController.currentZoom;

        float normalizedZoom = (currentZoom - minZoom) / (maxZoom - minZoom);

        int spriteIndex = Mathf.RoundToInt(normalizedZoom * (markerSprites.Count - 1));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, markerSprites.Count - 1);

        if (markerImage.sprite != markerSprites[spriteIndex])
        {
            markerImage.sprite = markerSprites[spriteIndex];
        }
    }
}