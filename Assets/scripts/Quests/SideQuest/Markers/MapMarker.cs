using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MapMarker : MonoBehaviour
{
  public Vector2 gpsCoords;
  public DynamicMapController mapController;
  public int id = -1; // ID for the marker, used for tracking

  public bool GpsUsed = true; // Set to false for testing in the editor
  RectTransform  _rt;
  RectTransform  _mapRt;

  void Awake()
  {
    _rt = GetComponent<RectTransform>();

    // Find the DynamicMapController dynamically
    mapController = FindObjectOfType<DynamicMapController>();
    if (mapController == null)
    {
        Debug.LogError("DynamicMapController not found in the scene.");
        return;
    }

    _mapRt = mapController.mapImage.rectTransform;
  }

  void LateUpdate()
  {
    if (GpsUsed) {
      Locate();
    }
  }

  void Locate() 
  {

    float pctX = (gpsCoords.x - mapController.bottomLeftGPS.x)
               / (mapController.topRightGPS.x - mapController.bottomLeftGPS.x);
    float pctY = (gpsCoords.y - mapController.bottomLeftGPS.y)
               / (mapController.topRightGPS.y - mapController.bottomLeftGPS.y);

    // 2) Convert into pixels in the map's local space
    //    Use the map's current rect dimensions, which account for its scale.
    float currentMapWidth = _mapRt.rect.width;
    float currentMapHeight = _mapRt.rect.height;

    float px = pctX * currentMapWidth - currentMapWidth * _mapRt.pivot.x;
    float py = pctY * currentMapHeight - currentMapHeight * _mapRt.pivot.y;

    // 3) Apply the position directly to the marker
    _rt.anchoredPosition = new Vector2(px, py);
  }
}
