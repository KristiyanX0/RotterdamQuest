using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcherScreenTap
{
    private ImageSwitcherManager manager;
    private int currentIndex = 0;

    public ImageSwitcherScreenTap(ImageSwitcherManager manager)
    {
        this.manager = manager;

        if (manager.images.Length > 0)
        {
            SetImage(currentIndex);
        }
    }

    public void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleScreenTap(Input.GetTouch(0).position);
        }
        else if (Input.GetMouseButtonDown(0)) // For desktop testing
        {
            HandleScreenTap(Input.mousePosition);
        }
    }

    private void HandleScreenTap(Vector2 screenPosition)
    {
        Debug.Log("Tapped at: " + screenPosition);
        if (screenPosition.x < Screen.width / 2)
        {
            PreviousImage();
        }
        else
        {
            NextImage();
        }
    }

    private void NextImage()
    {
        if (currentIndex < manager.images.Length - 1)
        {
            currentIndex++;
            SetImage(currentIndex);
        } else {
            // Optionally, loop back to the first image
            currentIndex = 0;
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
