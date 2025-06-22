// -------------------------- //
// ------- DEPRECATED ------- //
// -------------------------- //

using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class MapFollowPlayer_copy : MonoBehaviour
{
    public RectTransform mapRectTransform;          // Reference to the map's RectTransform
    public RectTransform playerPinRectTransform;    // Reference to the player's pin RectTransform
    public float playerOffset = -100f;              // Vertical offset for the player position
    public int cityChoice = 0;                      // 0 for Rotterdam, 1 for Sofia

    private Vector2 bottomLeftGPS;                  // Bottom-left GPS corner of the map (longitude, latitude)
    private Vector2 topRightGPS;                    // Top-right GPS corner of the map (longitude, latitude)
    private Vector2 mapSize;                        // Size of the map in Unity units (width and height)

    void Start()
    {
        // Set GPS boundaries based on cityChoice
        SetCityGPSBounds();

        // Start GPS service
        StartCoroutine(StartGPS());

        // Get the size of the map in Unity units
        mapSize = new Vector2(mapRectTransform.rect.width, mapRectTransform.rect.height);

        // Center the player icon with an offset (slightly below center)
        playerPinRectTransform.anchoredPosition = new Vector2(0, playerOffset);
    }

    void SetCityGPSBounds()
    {
        if (cityChoice == 0) // Rotterdam Center
        {
            bottomLeftGPS = new Vector2(4.4619188983712155f, 51.907064240306546f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(4.494258011211837f, 51.927907343742206f);    // Top-right corner (longitude, latitude)
        }
        else if (cityChoice == 1) // Studentski grad
        {
            bottomLeftGPS = new Vector2(23.336951f, 42.644877f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(23.345809f, 42.654092f);   // Top-right corner (longitude, latitude)
        }
        else if (cityChoice == 2) // Rotterdam-West
        {
            bottomLeftGPS = new Vector2(4.409512f, 51.911768f); // Bottom-left corner (longitude, latitude)
            topRightGPS = new Vector2(4.434574f, 51.922912f);   // Top-right corner (longitude, latitude)
        }
        else
        {
            Debug.LogWarning("Invalid cityChoice selected. Please use 0 for Rotterdam Center, 1 for Studentski grad, or 2 for Rotterdam-West.");
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
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Get the player's current GPS coordinates
            float playerLongitude = Input.location.lastData.longitude;
            float playerLatitude = Input.location.lastData.latitude;

            // Debug log for the player's current location
            Debug.Log($"Player GPS Location: Longitude = {playerLongitude}, Latitude = {playerLatitude}");

            // Calculate the new map position based on the player's location
            Vector2 mapPosition = CalculateMapPosition(playerLongitude, playerLatitude);

            // Update the map's anchored position to keep the player at the offset position
            mapRectTransform.anchoredPosition = mapPosition;
        }
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

        return new Vector2(offsetX, offsetY + playerOffset);
    }
}
