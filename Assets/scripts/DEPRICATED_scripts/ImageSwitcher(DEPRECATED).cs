using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher_Deprecated : MonoBehaviour
{
    public Sprite[] images; // Array to hold your images
    private int currentIndex = 0; // Index of the current image
    private Image imageComponent; // Reference to the UI Image component
    public Button nextSceneButton; // Reference to the Button for scene transition
    public Button[] ListOfButtons; // Reference to the Button for Google Maps

    void Start()
    {
        imageComponent = GetComponent<Image>();

        if (images.Length > 0)
        {
            imageComponent.sprite = images[currentIndex];
            imageComponent.preserveAspect = true;
        }

        // Make sure the buttons are hidden at the start
        nextSceneButton.gameObject.SetActive(false);
        for (int i = 0; i < ListOfButtons.Length; i++)
        {
            ListOfButtons[i].gameObject.SetActive(false);
        }
    }

    void Update()
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

    void HandleScreenTap(Vector2 screenPosition)
    {
        // Determine if the click/tap was on the left or right side of the screen
        if (screenPosition.x < Screen.width / 2)
        {
            PreviousImage(); // Move to the previous image
        }
        else
        {
            NextImage(); // Move to the next image
        }
    }

    void NextImage()
    {
        if (currentIndex < images.Length - 1)
        {
            currentIndex++;
            imageComponent.sprite = images[currentIndex];
        }
        else
        {
            // If it's the final image, show the buttons
            imageComponent.gameObject.SetActive(false);
            nextSceneButton.gameObject.SetActive(true);
            for (int i = 0; i < ListOfButtons.Length; i++)
            {
                ListOfButtons[i].gameObject.SetActive(true);
            }
        }
    }

    void PreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            imageComponent.sprite = images[currentIndex];

            // If going back from the final image, re-enable the image
            if (!imageComponent.gameObject.activeSelf)
            {
                imageComponent.gameObject.SetActive(true);
                nextSceneButton.gameObject.SetActive(false);
                for (int i = 0; i < ListOfButtons.Length; i++)
                {
                    ListOfButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
