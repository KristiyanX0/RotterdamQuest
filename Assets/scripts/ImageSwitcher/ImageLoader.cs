using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ImageLoader : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Array to hold the loaded sprites.")]
    public Sprite[] images;

    [Tooltip("Index to select the correct image from the array.")]
    public int currentIndex;

    void Start()
    {
        SetImage(currentIndex);
    }

    void NextImage() {
        currentIndex++;
        SetImage(currentIndex);
    }

    public void SetImage(int index) {
        currentIndex = index;
        if (images != null && images.Length > 0 && index >= 0 && index < images.Length)
        {   
            this.GetComponent<UnityEngine.UI.Image>().sprite = images[index];
        }
        else
        {
            Debug.Log("Invalid image index: " + index);
        }
    }
}
