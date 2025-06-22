using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SequenceManager : MonoBehaviour
{
    public UnityEvent onSequenceEnd;
    public UnityEvent onSequenceUpdate;
    // public UnityEvent onSequenceStart;
    public List<GameObject> GameObjectElement; // List of GameObjectElements in sequence
    public Button forwardButton;
    public Button backButton;
    private int sideQuestIndex = -1;
    private int currentIndex = 0;

    void Start()
    {
        // Disable all GameObjectElement initially except the first one
        for (int i = 0; i < GameObjectElement.Count; i++)
        {
            GameObjectElement[i].gameObject.SetActive(i == 0);
        }
        GoToIndex(0);

        // Add listeners to arrow buttons
        // forwardButton.onClick.AddListener(() => GoForward());
        // backButton.onClick.AddListener(() => GoBack());
    }

    public int getIndex() {
        return currentIndex;
    }

    public void GoForward()
    {
        GoToIndex(currentIndex + 1);
    }

    public void GoBack()
    {
        GoToIndex(currentIndex - 1);
    }

    public void GoToIndex(int index)
    {
        backButton.gameObject.SetActive(true);
        forwardButton.gameObject.SetActive(true);

        if (index == GameObjectElement.Count - 1)
        {
            onSequenceEnd?.Invoke();
        }
        if (index < 0 || index >= GameObjectElement.Count)
        {
            Debug.LogWarning("Index is out of range.");
            return;
        }
        if (index <= 0)
        {
            Debug.Log("Disable back button");
            backButton.gameObject.SetActive(false);
        }
        if (index >= GameObjectElement.Count - 1)
        {
            Debug.Log("Disable forward button");
            forwardButton.gameObject.SetActive(false);
        }

        Debug.Log("GoToIndex to index: " + index);
        GameObjectElement[currentIndex].SetActive(false);
        currentIndex = index;
        GameObjectElement[currentIndex].SetActive(true);
        onSequenceUpdate?.Invoke();

        // onSequenceStart?.Invoke();
    }
}

