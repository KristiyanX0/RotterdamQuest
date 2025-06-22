using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaneDetectionToggle : MonoBehaviour
{
    private ARPlaneManager planeManager;
    [SerializeField]
    private Button button;

    // Start is called before the first frame update
    private void Start()
    {
        planeManager = GetComponent<ARPlaneManager>();
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Plane Detection: OFF";
    }

    public void TogglePlaneDetection()
    {
        planeManager.enabled = !planeManager.enabled;
        if (planeManager.enabled) {
            SetAllPlanesActive(true);
        } else {
            SetAllPlanesActive(false);
        }

        button.GetComponentInChildren<TextMeshProUGUI>().text = "Plane Detection: " + (planeManager.enabled ? "ON" : "OFF");
    }

    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
