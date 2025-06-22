using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject;

    // Function to toggle the object
    public void Toggle()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
