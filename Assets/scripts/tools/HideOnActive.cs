using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideOnActive : MonoBehaviour
{
    public List<GameObject> objs;
    public List<GameObject> objectsToToggle;

    private void Update()
    {
        bool anyActive = false;
        foreach (GameObject obj in objs)
        {
            if (obj.activeSelf)
            {
                anyActive = true;
                break;
            }
        }

        if (anyActive)
        {
            SetInactive();
        }
        else
        {
            SetActive();
        }
    }

    private void SetActive()
    {
        foreach (GameObject button in objectsToToggle)
        {
            button.SetActive(true);
        }
    }

    private void SetInactive()
    {
        foreach (GameObject button in objectsToToggle)
        {
            button.SetActive(false);
        }
    }
}
