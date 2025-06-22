using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARPrefabPlacer_copy2 : MonoBehaviour
{
    public Button button; // Button to place the prefab
    public GameObject prefab; // Prefab to instantiate
    public GameObject placementIndicator; // Placement indicator
    public Camera arCamera; // Assign the AR camera in the Inspector
    public LayerMask placementLayerMask; // Layer to detect for placement (e.g., ground or AR plane)
    public Transform fallbackTransform; // Fallback transform for placement

    private bool isPlacementValid = false; // Tracks if a valid placement position is found
    private Vector3 placementPosition; // Current placement position
    private Quaternion placementRotation; // Current placement rotation

    void Start() 
    {
        placementIndicator.SetActive(true);
        placementIndicator.transform.position = fallbackTransform.position;
        placementIndicator.transform.rotation = fallbackTransform.rotation;
    }

    void Update()
    {
        // Check for screen taps or mouse clicks
        if (IsTapOrClick())
        {
            HandleTapOrClick();
        }

        // Update placement indicator dynamically in valid cases
        if (isPlacementValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.position = placementPosition;
            placementIndicator.transform.rotation = placementRotation;
        }
    }

    private void HandleTapOrClick()
    {
        // Raycast from the screen point (mouse or touch)
        Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayerMask))
        {
            // If valid, update placement position and rotation
            placementPosition = hit.point;
            placementRotation = Quaternion.LookRotation(hit.normal);
            isPlacementValid = true;
            Debug.Log("Indicator placed at: " + placementPosition);
        }
        else
        {
            // If invalid, mark as not valid
            isPlacementValid = false;
            Debug.LogWarning("No valid placement point found at click/tap.");
        }
    }

    public void TogglePrefab()
    {
        if (isPlacementValid)
        {
            // Place the prefab at the current placement position
            Instantiate(prefab, placementPosition, placementRotation);
            Debug.Log("Prefab placed dynamically at: " + placementPosition);
        }
        else
        {
            // Use fallback transform for placement
            Instantiate(prefab, fallbackTransform.position, fallbackTransform.rotation);
            Debug.LogWarning("No valid placement position found. Using fallback transform position.");
        }
    }

    private bool IsTapOrClick()
    {
        // For mouse input (PC)
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        // For touch input (Mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
            {
                return true;
            }
        }

        return false;
    }
}
