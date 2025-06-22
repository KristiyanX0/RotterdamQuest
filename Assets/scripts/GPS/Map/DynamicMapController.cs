using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class DynamicMapController : MonoBehaviour
{
    // Use a single Image component for the map
    public Image mapImage;
    // A list of sprites for different zoom levels
    public List<Sprite> mapSprites;
    public RectTransform playerPinRectTransform;    // Reference to the player's pin RectTransform
    public float playerOffsetY;                       // Vertical offset for the player position
    public float playerOffsetX;

    // City choice for debugging purposes:
    // 0: Rotterdam Center, 1: Studentski grad, 2: Rotterdam-West, 3: Plovdiv, 4: Plovdiv 2
    public int cityChoice = 0;

    // GPS and zoom settings
    public float minZoom { get; private set; }
    public float maxZoom { get; private set; }
    public float zoomSpeed { get; private set; }
    public float currentZoom { get; private set; }
    float lastLogTime = 0;

    // GPS boundaries for the map
    public Vector2 bottomLeftGPS;
    public Vector2 topRightGPS;
    public Vector2 mapSize;

    void Start()
    {
        minZoom = 0.5f;
        maxZoom = 2.0f;
        zoomSpeed = 0.1f;
        currentZoom = 1.0f;

        // Set GPS boundaries based on the selected city (for debugging purposes)
        SetCityGPSBounds();

        // Start GPS service
        StartCoroutine(StartGPS());

        // Initialize the map sprite with the lowest zoom sprite (if available)
        if (mapImage != null && mapSprites != null && mapSprites.Count > 0)
        {
            mapImage.sprite = mapSprites[0];
        }

        // Center the player icon with an offset
        playerPinRectTransform.anchoredPosition = new Vector2(playerOffsetX, playerOffsetY);

        // Calculate initial map size
        UpdateMapSize();
    }

    void SetCityGPSBounds()
    {
        if (cityChoice == 0) // Rotterdam Center
        {
            bottomLeftGPS = new Vector2(4.4619188983712155f, 51.907064240306546f);
            topRightGPS = new Vector2(4.494258011211837f, 51.927907343742206f);
        }
        else if (cityChoice == 1) // Studentski grad
        {
            bottomLeftGPS = new Vector2(23.336951f, 42.644877f);
            topRightGPS = new Vector2(23.345809f, 42.654092f);
        }
        else if (cityChoice == 2) // Rotterdam-West
        {
            bottomLeftGPS = new Vector2(4.409512f, 51.911768f);
            topRightGPS = new Vector2(4.434574f, 51.922912f);
        }
        else if (cityChoice == 3) // Plovdiv
        {
            bottomLeftGPS = new Vector2(24.743331f, 42.126500f);
            topRightGPS = new Vector2(24.751009f, 42.131064f);
        }
        else if (cityChoice == 4) // Plovdiv 2
        {
            bottomLeftGPS = new Vector2(24.726873f, 42.135300f);
            topRightGPS = new Vector2(24.739490f, 42.123078f);
        }
        else
        {
            Debug.LogWarning("Invalid cityChoice selected. Defaulting to Rotterdam Center.");
            bottomLeftGPS = new Vector2(4.4619188983712155f, 51.907064240306546f);
            topRightGPS = new Vector2(4.494258011211837f, 51.927907343742206f);
        }
    }

    IEnumerator StartGPS()
    {
    #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitForSeconds(1);
        }
    #endif

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS is not enabled.");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location.");
            yield break;
        }
    }

    void Update()
    {
        Vector2 anchoredPosition = playerPinRectTransform.anchoredPosition;

        // Modify its x and y components
        playerOffsetX = anchoredPosition.x;
        playerOffsetY = anchoredPosition.y;

        HandlePinchZoom();

        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Get the player's current GPS coordinates
            float playerLongitude = Input.location.lastData.longitude;
            float playerLatitude = Input.location.lastData.latitude;

            if (Time.time - lastLogTime > 3)
            {
                Debug.Log($"Player GPS Location: Longitude = {playerLongitude}, Latitude = {playerLatitude}");
                lastLogTime = Time.time;
            }

            // Calculate the new map position based on the player's location
            Vector2 mapPosition = CalculateMapPosition(playerLongitude, playerLatitude);

            // Update the map image's RectTransform position
            mapImage.rectTransform.anchoredPosition = mapPosition;
        }
    }

    void HandlePinchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate previous positions of touches
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;
            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            // Difference in the distances between frames
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            // Adjust zoom based on pinch and clamp the value
            currentZoom += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Update the map image scale
            mapImage.rectTransform.localScale = new Vector3(currentZoom, currentZoom, 1);
            UpdateMapSize();

            // If a list of sprites is available, select the sprite based on zoom level.
            if (mapSprites != null && mapSprites.Count > 0)
            {
                // Normalize currentZoom to a value between 0 and 1
                float normalizedZoom = (currentZoom - minZoom) / (maxZoom - minZoom);
                // Calculate the index based on the normalized zoom value
                int spriteIndex = Mathf.RoundToInt(normalizedZoom * (mapSprites.Count - 1));
                spriteIndex = Mathf.Clamp(spriteIndex, 0, mapSprites.Count - 1);

                // Only update the sprite if it has changed
                if (mapImage.sprite != mapSprites[spriteIndex])
                {
                    mapImage.sprite = mapSprites[spriteIndex];
                }
            }
        }
    }

    void UpdateMapSize()
    {
        // Update the map size based on the current scale
        RectTransform rt = mapImage.rectTransform;
        mapSize = new Vector2(
            rt.rect.width * rt.localScale.x, 
            rt.rect.height * rt.localScale.y
        );
    }

    // Calculate the map's position based on GPS coordinates
    Vector2 CalculateMapPosition(float longitude, float latitude)
    {
        float percentX = (longitude - bottomLeftGPS.x) / (topRightGPS.x - bottomLeftGPS.x);
        float percentY = (latitude - bottomLeftGPS.y) / (topRightGPS.y - bottomLeftGPS.y);

        // Debuging and for testing
        float offsetX = -(percentX * mapSize.x - (mapSize.x * mapImage.rectTransform.pivot.x));
        float offsetY = -(percentY * mapSize.y - (mapSize.y * mapImage.rectTransform.pivot.y));

        float offsetPercentageX = 0.0f;
        float offsetPercentageY = -0.025f;

        float horizontalOffset = mapSize.x * offsetPercentageX;
        float verticalOffset = mapSize.y * offsetPercentageY;

        return new Vector2(offsetX + playerOffsetX + horizontalOffset,
                        offsetY + playerOffsetY + verticalOffset);
    }
}
