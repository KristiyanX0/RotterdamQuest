using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;


public class MapFollowPlayer : MonoBehaviour
{
    public List<RectTransform> mapImages;
    public RectTransform mapRectTransform;          // Reference to the map's RectTransform
    public RectTransform playerPinRectTransform;    // Reference to the player's pin RectTransform
    public float playerOffsetY;              // Vertical offset for the player position
    public float playerOffsetX;   
    public int LocationChoice = 0;                      // 0 for Rotterdam, 1 for Sofia, 2 for Rotterdam-West

    // Zoom settings
    public float minZoom = 0.5f;  // Minimum zoom scale
    public float maxZoom = 2.0f;  // Maximum zoom scale
    public float zoomSpeed = 0.1f;

    
    private Vector2 bottomLeftGPS;                  // Bottom-left GPS corner of the map (longitude, latitude)
    private Vector2 topRightGPS;                    // Top-right GPS corner of the map (longitude, latitude)
    private Vector2 mapSize;                        // Size of the map in Unity units (width and height)
    private Vector3 initialScale;                   // Initial scale of the map
    private float currentZoom = 1.0f;               // Current zoom level
    float lastLogTime = 0;                          // Time for debug logging
    delegate void HandlePinchZoomFunction();
    HandlePinchZoomFunction HandlePinchZoom;

    private int currentIndexMap = 0;

    void Start()
    {
        // Set GPS boundaries based on LocationChoice
        SetCityGPSBounds();

        // Start GPS service
        StartCoroutine(StartGPS());

        // Enable only the first map image
        foreach (var image in mapImages)
        {
            image.gameObject.SetActive(false);
        }
        mapImages[0].gameObject.SetActive(true);

        mapRectTransform = mapImages[0];
        // Get the size of the map in Unity units
        initialScale = mapRectTransform.localScale;
        UpdateMapSize();

        HandlePinchZoom = HandlePinchZoomTwoPics;

        // Center the player icon with an offset (slightly below center)
        playerPinRectTransform.anchoredPosition = new Vector2(playerOffsetX, playerOffsetY);
    }

    void SetCityGPSBounds()
    {
        if (LocationChoice == 0) // Rotterdam Center
        {
            bottomLeftGPS = new Vector2(4.4619188983712155f, 51.907064240306546f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(4.494258011211837f, 51.927907343742206f);    // Top-right corner (longitude, latitude)
        }
        else if (LocationChoice == 1) // Studentski grad
        {
            bottomLeftGPS = new Vector2(23.336951f, 42.644877f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(23.345809f, 42.654092f);   // Top-right corner (longitude, latitude)
        }
        else if (LocationChoice == 2) // Rotterdam-West
        {
            bottomLeftGPS = new Vector2(4.409512f, 51.911768f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(4.434574f, 51.922912f);   // Top-right corner (longitude, latitude)
        }
        else if (LocationChoice == 3) // Plovdiv
        {
            bottomLeftGPS = new Vector2(24.743331f, 42.126500f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(24.751009f, 42.131064f);   // Top-right corner (longitude, latitude)
        }
        else if (LocationChoice == 4) // Plovdiv 2
        {
            bottomLeftGPS = new Vector2(24.726873f, 42.135300f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(24.739490f, 42.123078f);   // Top-right corner (longitude, latitude)
        }
        else if (LocationChoice == 5) // FMI
        {
            bottomLeftGPS = new Vector2(23.326639f, 42.677222f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(23.335972f, 42.671500f);   // Top-right corner (longitude, latitude)
        }
        else
        {
            Debug.LogWarning("Invalid LocationChoice selected. Please use 0 for Rotterdam Center, 1 for Studentski grad, or 2 for Rotterdam-West.");
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
        // Get the current anchoredPosition
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

            // Debug log for the player's current location
            if (Time.time - lastLogTime > 3)
            {
                Debug.Log($"Player GPS Location: Longitude = {playerLongitude}, Latitude = {playerLatitude}");
                lastLogTime = Time.time;
            }

            // Calculate the new map position based on the player's location
            Vector2 mapPosition = CalculateMapPosition(playerLongitude, playerLatitude);

            // Update the map's anchored position to keep the player at the offset position
            mapRectTransform.anchoredPosition = mapPosition;
        }
    }

    void HandlePinchZoomSmooth()
    {
        if (Input.touchCount == 2 && 
        UITools.IsPointerOverUIObject(mapRectTransform, Input.GetTouch(0).fingerId) && 
        UITools.IsPointerOverUIObject(mapRectTransform, Input.GetTouch(1).fingerId))
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate the initial distance between touches
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                currentZoom = mapRectTransform.localScale.x; // Current zoom based on scale
            }

            // Calculate the current distance between touches
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;
                float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;

                float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                // Difference in the distances between frames
                float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

                // Adjust zoom based on pinch
                currentZoom += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
                currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

                // Apply zoom
                mapRectTransform.localScale = new Vector3(currentZoom, currentZoom, 1);

                // Update the map size to match the new scale
                UpdateMapSize();
            }
        }
    }

    void HandlePinchZoomTwoPics()
    {
        if (Input.touchCount == 2 && 
        UITools.IsPointerOverUIObject(mapRectTransform, Input.GetTouch(0).fingerId) && 
        UITools.IsPointerOverUIObject(mapRectTransform, Input.GetTouch(1).fingerId))
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate the current distance between touches
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            // Difference in the distances between frames
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            // Adjust zoom based on pinch
            currentZoom += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Check if the zoom level has reached its maximum
            if (currentZoom > (maxZoom + minZoom) / 2.0f)
            {
                // Switch to the second map if not already active
                SwitchMap(mapImages[1]);

                // Resize the second map to match the maximum zoom level
                mapRectTransform.localScale = new Vector3(currentZoom, currentZoom, 1);
                UpdateMapSize();
            }
            else
            {
                SwitchMap(mapImages[0]);

                mapRectTransform.localScale = new Vector3(currentZoom, currentZoom, 1);
                UpdateMapSize();
            }
        }
    }

    void SwitchMap(RectTransform newMap)
    {
        if (mapRectTransform != newMap)
        {
            foreach (var m in mapImages)
            {
                m.gameObject.SetActive(false);
            }

            mapRectTransform = newMap;
            mapRectTransform.gameObject.SetActive(true);

            Debug.Log("Switched to new map");

        }
    }

    void UpdateMapSize()
    {
        // Update the map size based on the current scale
        mapSize = new Vector2(
            mapRectTransform.rect.width * mapRectTransform.localScale.x,
            mapRectTransform.rect.height * mapRectTransform.localScale.y
        );
    }

    // Function to calculate map position based on GPS coordinates
    Vector2 CalculateMapPosition(float longitude, float latitude)
    {
        // Calculate percentage of player's GPS position within the map's GPS boundaries
        float percentX = (longitude - bottomLeftGPS.x) / (topRightGPS.x - bottomLeftGPS.x);
        float percentY = (latitude - bottomLeftGPS.y) / (topRightGPS.y - bottomLeftGPS.y);

        // Calculate offset in Unity units relative to map size
        float offsetX = -(percentX * mapSize.x - (mapSize.x * mapRectTransform.pivot.x));
        float offsetY = -(percentY * mapSize.y - (mapSize.y * mapRectTransform.pivot.y));

        return new Vector2(offsetX + playerOffsetX, offsetY + playerOffsetY);
    }
}
