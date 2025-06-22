using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandleDaze : MonoBehaviour
{
    public static HandleDaze instance;
    public List<String> buttonNames;
    private List<Button> buttonListener;

    public int toBeDazed = 1;
    private int attackCounter = 0;
    private bool isWaitingToDazeAgain = false;
    public bool isDazed { get; private set; }
    public UnityAction onDaze;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool CanBeDazed() {
        return (attackCounter >= toBeDazed) && !isDazed && !isWaitingToDazeAgain;
    }

    public void GetDazedIfPossible(UnityAction startDaze = null) {
        incrementAttackCounter();
        if (CanBeDazed())
        {
            isDazed = true;
            startDaze?.Invoke();
        }
    }

    void Start()
    {   
        buttonListener = new List<Button>();
        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (string buttonName in buttonNames)
        {
            foreach (Button button in allButtons)
            {
                if (button.gameObject.name == buttonName)
                {
                    buttonListener.Add(button);
                    break; // Stop searching once the button is found.
                }
            }
        }

        buttonListener.ForEach(button => button.onClick.AddListener(() => GetDazedIfPossible(onDaze)));
        isDazed = false;
    }
    

    public void stopDaze() {
        isDazed = false;
        attackCounter = 0;
        StartCoroutine(waitToDazeAgain());
    }

    IEnumerator waitToDazeAgain() {
        isWaitingToDazeAgain = true;
        buttonListener.ForEach(button => button.interactable = false);
        yield return new WaitForSeconds(10f);
        isWaitingToDazeAgain = false;
        buttonListener.ForEach(button => button.interactable = true);
    }

    public void incrementAttackCounter() {
        attackCounter++;
    }

}
