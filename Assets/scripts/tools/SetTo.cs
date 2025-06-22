using UnityEngine;

public class SetTo : MonoBehaviour
{
    public void SetToFalse(GameObject target_object)
    {
        if (target_object != null)
        {
            target_object.SetActive(false);
        }
    }

    public void SetToTrue(GameObject target_object)
    {
        if (target_object != null)
        {
            target_object.SetActive(true);
        }
    }

}
