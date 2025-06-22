using UnityEngine;

public class Toggle : MonoBehaviour
{
    public void toggle(GameObject target_object)
    {
        if (target_object != null)
        {
            target_object.SetActive(!target_object.activeSelf);
        }
    }

}
