using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ARPrefabPlacer_copy : MonoBehaviour
{
    public Button button;
    public GameObject prefab; // Assign your prefab in the Inspector
    public Transform placementTransform; // Assign a transform for placement

    public GameObject healthBar;

    private GameObject placedObject; // Keeps reference to the placed prefab

    // Function to be linked with the button

    void Start()
    {
        // button.targetGraphic.color = Color.red;
        // button.GetComponentInChildren<TextMeshProUGUI>().text = "Object: " + ((placedObject != null) ? "ON" : "OFF");
    }
    public void TogglePrefab()
    {
        if (placedObject != null)
        {
            // Destroy the existing prefab
            Destroy(placedObject);
            button.targetGraphic.color = Color.red;
            placedObject = null;
        }
        else
        {
            // Place the prefab at the specified position and rotation
            button.targetGraphic.color = Color.green;
            placedObject = Instantiate(prefab, placementTransform.position, placementTransform.rotation);

        }
        // button.GetComponentInChildren<TextMeshProUGUI>().text = "Object: " + ((placedObject != null) ? "ON" : "OFF");
        
    }
}
