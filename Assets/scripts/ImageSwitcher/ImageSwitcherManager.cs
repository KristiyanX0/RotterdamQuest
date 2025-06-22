using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class ImageSwitcherManager : MonoBehaviour
{
    public UnityEvent OnFinalImage;
    public Sprite[] images; // Array to hold your images
    public Button leftButton; // Button to move to the previous image
    public Button rightButton; // Button to move to the next image
    public Button[] listOfButtons; // Buttons for additional actions
    public int navigationMode = 2; // 1 for screen tap, 2 for button navigation

    private ImageSwitcherScreenTap screenTapSwitcher;
    private ImageSwitcherButtonBased buttonSwitcher;

    void Start()
    {
        if (navigationMode == 1)
        {
            screenTapSwitcher = new ImageSwitcherScreenTap(this);
        }
        else if (navigationMode == 2)
        {
            buttonSwitcher = new ImageSwitcherButtonBased(this);
        }
    }

    void Update()
    {
        if (navigationMode == 1)
        {
            screenTapSwitcher.Update();
        }
        else if (navigationMode == 2)
        {
            buttonSwitcher.Update();
        }
    }


    public void UpdateUIState(int currentIndex, int totalImages)
    {
        // bool atLastImage = (currentIndex == totalImages - 1);

        // foreach (Button btn in listOfButtons)
        // {
        //     btn.gameObject.SetActive(atLastImage);
        // }

        if (currentIndex == totalImages - 1)
        {
            OnFinalImage.Invoke();
        }
    }
}
