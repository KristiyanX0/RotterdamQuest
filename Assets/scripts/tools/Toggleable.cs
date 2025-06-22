using UnityEngine;

public class Toggleable : MonoBehaviour
{
    private GameObject targetObject;

    void Awake() {
        targetObject = this.gameObject;
    }

    public void Toggle()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
