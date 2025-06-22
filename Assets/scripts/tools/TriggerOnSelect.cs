using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TriggerOnSelect : MonoBehaviour
{
    [Tooltip("Assign the Button you want to press automatically")]
    public Button otherButton;
    private Button thisButton;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(onClick);    
    }

    public void onClick()
    {
        if (otherButton != null)
            otherButton.onClick.Invoke();
    }
}
