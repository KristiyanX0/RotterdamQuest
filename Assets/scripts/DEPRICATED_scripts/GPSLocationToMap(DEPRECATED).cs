// -------------------------- //
// ------- DEPRECATED ------- //
// -------------------------- //

using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class GPSLocationToMap : MonoBehaviour
{
    public RectTransform mapRectTransform;  // Reference to the map's RectTransform
    public RectTransform playerPinRectTransform;  // Reference to the player's pin RectTransform

    // Define the GPS boundaries of the map (lon/lat)
    public Vector2 bottomLeftGPS = new Vector2(23.336951f, 42.644877f);  // Bottom-left corner (lon, lat)
    public Vector2 topRightGPS = new Vector2(23.345809f, 42.654092f);    // Top-right corner (lon, lat)

    private Vector2 mapSize;  // Size of the map in Unity units (width and height)

    void Start()
    {
        // Start GPS service
        StartCoroutine(StartGPS());

        // Get the size of the map in Unity units (width and height)
        mapSize = new Vector2(mapRectTransform.rect.width, mapRectTransform.rect.height);
    }

    IEnumerator StartGPS()
    {
        // Request location permission for Android
        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitForSeconds(1);  // Wait for permission to be granted
        }
        #endif

        // Check if GPS is enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS is not enabled.");
            yield break;
        }

        // Start the GPS service
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
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Get the player's current GPS coordinates
            float playerLongitude = Input.location.lastData.longitude;
            float playerLatitude = Input.location.lastData.latitude;

            // Print the player's current location for debugging
            Debug.Log($"Player GPS Location: Longitude = {playerLongitude}, Latitude = {playerLatitude}");

            // Convert the player's GPS position to Unity map coordinates
            Vector2 mapPosition = ConvertGPSToMap(playerLongitude, playerLatitude);

            // Update the player's pin position on the map
            playerPinRectTransform.anchoredPosition = mapPosition;
        }
    }

    // Function to convert GPS coordinates (longitude, latitude) to Unity map coordinates
    Vector2 ConvertGPSToMap(float longitude, float latitude)
    {
        // Calculate the percentage position of the player's GPS within the map's GPS bounds
        float percentX = (longitude - bottomLeftGPS.x) / (topRightGPS.x - bottomLeftGPS.x);
        float percentY = (latitude - bottomLeftGPS.y) / (topRightGPS.y - bottomLeftGPS.y);

        // Convert the percentage position to Unity units on the map
        float mapPosX = percentX * mapSize.x - (mapSize.x * mapRectTransform.pivot.x);
        float mapPosY = percentY * mapSize.y - (mapSize.y * mapRectTransform.pivot.y);

        return new Vector2(mapPosX, mapPosY);
    }
}
