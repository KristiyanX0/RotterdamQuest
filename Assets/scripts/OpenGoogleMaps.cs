using UnityEngine;

public class OpenGoogleMaps : MonoBehaviour
{
    // Coordinates for Google Maps (set to 51°55'01.4"N 4°28'24.2"E)
    public float latitude = 51.91706f;
    public float longitude = 4.47339f;

    // Method to open Google Maps with the specified coordinates
    public void OpenMaps()
    {
        string url = $"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}";
        Application.OpenURL(url);
    }
}
