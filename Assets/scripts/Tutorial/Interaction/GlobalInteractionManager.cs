using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public static class GlobalInteractionManager
{
    // Dictionary of objects that are currently being listened for.
    // Initially, the boolean is false (not yet interacted).
    private static Dictionary<GameObject, bool> _listeningObjects = new Dictionary<GameObject, bool>();

    // List of objects that have been interacted with.
    private static List<GameObject> _interactedObjects = new List<GameObject>();

    // Flag to ensure our updater is created only once.
    private static bool _initialized = false;
    private static InteractionUpdater _updater;

    /// <summary>
    /// Call this method to start listening for interactions on the given game object.
    /// The object should have a collider and have its Raycast Target enabled.
    /// </summary>
    public static void RegisterObject(GameObject obj)
    {
        if (!_initialized)
        {
            Initialize();
        }
        // Only register if not already registered or already interacted with.
        if (!_listeningObjects.ContainsKey(obj) && !_interactedObjects.Contains(obj))
        {
            _listeningObjects.Add(obj, false);
        }
    }

    /// <summary>
    /// Check if the given object was interacted with.
    /// </summary>
    public static bool WasInteracted(GameObject obj)
    {
        return _interactedObjects.Contains(obj);
    }

    /// <summary>
    /// Creates a hidden GameObject with a MonoBehaviour to process input every frame.
    /// </summary>
    private static void Initialize()
    {
        GameObject updaterObj = new GameObject("InteractionUpdater");
        _updater = updaterObj.AddComponent<InteractionUpdater>();
        GameObject.DontDestroyOnLoad(updaterObj);
        _initialized = true;
    }

    /// <summary>
    /// Called every frame from our updater.
    /// Checks for user input and processes a raycast.
    /// </summary>
    public static void ProcessInput()
    {
        if (_listeningObjects.Count == 0)
            return;

        Vector3 screenInputPosition = Vector3.zero;
        bool inputDetected = false;

        // Check for mouse input.
        if (Input.GetMouseButtonDown(0))
        {
            screenInputPosition = Input.mousePosition;
            inputDetected = true;
        }
        // Check for touch input.
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            screenInputPosition = Input.GetTouch(0).position;
            inputDetected = true;
        }

        if (!inputDetected)
            return;

        // --- First: Try to raycast UI elements ---
        if (EventSystem.current != null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = screenInputPosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    Debug.Log("Hit UI element: " + result.gameObject.name);
                    GameObject hitObj = result.gameObject;
                    if (_listeningObjects.ContainsKey(hitObj))
                    {
                        _listeningObjects[hitObj] = true;
                        _interactedObjects.Add(hitObj);
                        _listeningObjects.Remove(hitObj);
                        return; // Exit after a successful UI interaction.
                    }
                }
            }
        }

        // --- Fallback: Raycast for 3D objects ---
        if (Camera.main == null)
        {
            Debug.LogWarning("No main camera found for raycasting.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenInputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (_listeningObjects.ContainsKey(hitObj))
            {
                _listeningObjects[hitObj] = true;
                _interactedObjects.Add(hitObj);
                _listeningObjects.Remove(hitObj);
                Debug.Log("Interacted with " + hitObj.name);
            }
            else
            {
                Debug.Log("Hit object " + hitObj.name + " is not registered.");
            }
        }
        else
        {
            Debug.Log("No object hit with Physics.Raycast.");
        }
    }
}
