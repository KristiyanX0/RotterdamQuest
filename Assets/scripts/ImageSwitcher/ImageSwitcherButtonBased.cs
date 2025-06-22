using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcherButtonBased
{
    private ImageSwitcherManager manager;
    private int currentIndex = 0;

    public ImageSwitcherButtonBased(ImageSwitcherManager manager)
    {
        this.manager = manager;

        if (manager.images.Length > 0)
        {
            SetImage(currentIndex);
        }

        // Set up button listeners
        manager.leftButton.onClick.AddListener(PreviousImage);
        manager.rightButton.onClick.AddListener(NextImage);
    }

    public void Update()
    {
        if (manager.navigationMode == 2)
        {
            // Update button visibility for button-based navigation
            manager.leftButton.gameObject.SetActive(currentIndex > 0 && currentIndex < manager.images.Length - 1);
            manager.rightButton.gameObject.SetActive(currentIndex < manager.images.Length - 1);
        }
        else
        {
            // Hide navigation buttons for screen-tap navigation
            manager.leftButton.gameObject.SetActive(false);
            manager.rightButton.gameObject.SetActive(false);
        }
    }

    private void NextImage()
    {
        if (currentIndex < manager.images.Length)
        {
            currentIndex++;
            SetImage(currentIndex);
        }
    }

    private void PreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            SetImage(currentIndex);
        }
    }

    private void SetImage(int index)
    {
        manager.GetComponent<Image>().sprite = manager.images[index];
        manager.UpdateUIState(index, manager.images.Length);
    }
}
