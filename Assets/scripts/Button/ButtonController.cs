using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    // Assign these in the Inspector
    public SequenceManager manager;
    public List<String> tags;
    public List<GameObject> buttonsToToggle;

    private void Update()
    {
        // Check if the GameObject this script is attached to is active
        if (tags.Contains(manager.GameObjectElement[manager.getIndex()].tag))
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
        foreach (GameObject button in buttonsToToggle)
        {
            button.SetActive(true);
        }
    }

    private void SetInactive()
    {
        foreach (GameObject button in buttonsToToggle)
        {
            button.SetActive(false);
        }
    }
}
