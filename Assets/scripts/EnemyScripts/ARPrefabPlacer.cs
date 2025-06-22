using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation; // For ARRaycastManager
using UnityEngine.XR.ARSubsystems;

public class ARPrefabPlacer : MonoBehaviour
{
    public GameObject prefab; // Prefab to instantiate
    public GameObject placementIndicator; // Placement indicator
    public Camera arCamera; // Assign the AR camera in the Inspector
    public ARRaycastManager arRaycastManager; // AR Raycast Manager for surface detection
    public Transform fallbackTransform; // Fallback transform for placement
    
    private bool isPlacementValid = false; // Tracks if a valid placement position is found
    private Vector3 placementPosition; // Current placement position
    private Quaternion placementRotation; // Current placement rotation
    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>(); // Store raycast hits

    private const float placementIndicatorSizeX = 0.6593134f;
    private const float placementIndicatorSizeY = 0.8290713f;

    private GameObject placedPrefab;

    void Start() 
    {
        placementIndicator.SetActive(true);
        placementIndicator.transform.position = fallbackTransform.position;
        placementIndicator.transform.rotation = fallbackTransform.rotation;
        placementIndicator.transform.localScale = prefab.transform.localScale;
    }

    void Update()
    {
        // Continuously check for a valid placement position
        // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        // {
        //     if (IsTouchOverUIObject(Input.GetTouch(0).position))
        //     {
        //         Debug.LogWarning("Touch is over UI object");
        //         return; // Ignore touch if it's over a UI object
        //     }
            isPlacementValid = TryGetPlacementPosition(out placementPosition, out placementRotation);
        // }

        if (isPlacementValid && placedPrefab == null)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.position = placementPosition + new Vector3(0, placementIndicator.GetComponent<Collider>().bounds.extents.y, 0);
            placementIndicator.transform.rotation = placementRotation;
            // placementIndicator.transform.localScale = new Vector3(placementIndicatorSizeX, placementIndicatorSizeY, placementIndicatorSizeX);
        }

        if (placedPrefab != null)
        {
            placementIndicator.SetActive(false);
        }
    }

    private bool TryGetPlacementPosition(out Vector3 position, out Quaternion rotation)
    {
        // Perform AR raycast from the center of the screen
        if (Application.platform == RuntimePlatform.Android)
        {

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

            if (arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
            {
                position = hits[0].pose.position;
                rotation = hits[0].pose.rotation;
                return true;
            }
        }
        
        position = Vector3.zero;
        rotation = Quaternion.identity;
        return false;
    }

    public void TogglePrefab()
    {
        placedPrefab = Instantiate(prefab, placementIndicator.transform.position, placementIndicator.transform.rotation );
    }

    // Method to check if the current input is over a UI element
    private bool IsTouchOverUIObject(Vector2 screenPosition)
    {

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        Debug.LogWarning("Raycast results count: " + raycastResults.Count);
        return raycastResults.Count > 0;
    }

}
