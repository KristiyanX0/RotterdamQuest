using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObjectOnPlane : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> PlacedPrefabsList = new List<GameObject>();

    [SerializeField]
    private int maxPlacedPrefabs = 1;
    private int currentPlacedPrefabs = 0;    
    [SerializeField]
    private GameObject DefaultPlaceblePrefab;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        touchPosition = default;

        // Check if there are any touches
        if (Input.touchCount > 0)
        {
            // Access the first touch only if it exists
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
        }
        
        return false;
    }


    private void Update()
    {
        if (maxPlacedPrefabs <= 0) return;

        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = hits[0].pose;
            if (currentPlacedPrefabs < maxPlacedPrefabs)
            {
                SpawnPrefab(hitPos);
            }
        }
    }


    public void SetPrefabType(GameObject prefab) 
    {
        DefaultPlaceblePrefab = prefab;
    }

    private void SpawnPrefab(Pose pose)
    {
        if (DefaultPlaceblePrefab == null)
        {
            Debug.LogError("DefaultPlaceblePrefab is not set.");
            return;
        }
        spawnedObject = Instantiate(DefaultPlaceblePrefab, pose.position, Quaternion.identity);
        PlacedPrefabsList.Add(spawnedObject);
        currentPlacedPrefabs++;
    }

}
