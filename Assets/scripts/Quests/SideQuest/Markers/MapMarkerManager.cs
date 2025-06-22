using UnityEngine;
using UnityEngine.UI;
using RotterdamQuestGameUtils;    // for SideQRewardData and ActualPair<,>
using System.Collections.Generic;

public class MapMarkerSpawner : MonoBehaviour
{
    public static MapMarkerSpawner instance;
    public SideQuestRewardsData rewardsData;      // ← your ScriptableObject asset
    public RectTransform markerPrefab;            // ← your diamond prefab (UI Image)
    public RectTransform mapRect;                // ← the map image RectTransform
    List<RectTransform> _instances = new List<RectTransform>();
    Dictionary<int, RectTransform> _markerInstances = new Dictionary<int, RectTransform>();

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        if (rewardsData == null || markerPrefab == null)
        {
            Debug.LogError("MapMarkerSpawner: missing references!");
            enabled = false;
            return;
        }
    }

    public void SpawnMarker(int id)
    {
        // Check if the marker already exists
        if (_markerInstances.ContainsKey(id))
        {
            Debug.LogWarning($"MapMarkerSpawner: Marker with ID {id} already exists.");
            return;
        }

        // Find the reward data by ID
        var reward = rewardsData.RewardsData.Find(r => r.id == id);
        if (reward == null)
        {
            Debug.LogWarning($"MapMarkerSpawner: No reward found with ID {id}");
            return;
        }

        // Retrieve GPS coordinates from the reward data
        Vector2 gpsCoords = new Vector2(reward.mapX, reward.mapY);

        // Instantiate the marker prefab
        RectTransform markerInstance = Instantiate(markerPrefab, mapRect);

        MapMarker mapMarker = markerPrefab.gameObject.GetComponent<MapMarker>();
        mapMarker.gpsCoords = gpsCoords;
        mapMarker.id = id;

        // Optionally, set additional properties (e.g., name)
        markerInstance.name = $"Marker_{id}";

        // Add the instance to the dictionary for tracking
        _markerInstances[id] = markerInstance;
    }

    public void UpdateMarkerPosition(int id, Vector2 newGpsCoords)
    {
        // Check if the marker exists
        if (!_markerInstances.TryGetValue(id, out var markerInstance))
        {
            Debug.LogWarning($"MapMarkerSpawner: No marker found with ID {id} to update.");
            return;
        }

        markerPrefab.gameObject.GetComponent<MapMarker>().gpsCoords = newGpsCoords;
    }
}
