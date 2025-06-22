using UnityEngine;
using System.Collections;
using System;

class GalleryService : MonoBehaviour
{
    private static string ALBUM_NAME = "Rotterdam_Quest";
    private static string FILE_NAME_START = "Rotterdam_Quest_Captured_Image_";

    public void saveImage(WebCamTexture webCamTexture) {
        if (webCamTexture == null)
        {
            Debug.LogError("WebCamTexture is null!");
            return;
        }

        Texture2D capturedImage = new Texture2D(webCamTexture.width, webCamTexture.height);
        capturedImage.SetPixels(webCamTexture.GetPixels());
        capturedImage.Apply();
        this.saveImage(capturedImage);
    }

    public void saveImage(Texture2D capturedImage)
    {
        string filePath = FILE_NAME_START + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        if (capturedImage == null)
        {
            Debug.LogError("Image is null");
            return;
        }
        StartCoroutine( TakeScreenshot(filePath, capturedImage) );

    }

    private IEnumerator TakeScreenshot(String filePath, Texture2D capturedImage)
    {
        yield return new WaitForEndOfFrame();

        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(capturedImage, ALBUM_NAME, filePath, 
        ( saved, path ) => { Debug.Log("Image is " + (saved ? "saved" : "not saved") + " to " + path + " on the device"); });

        Debug.LogWarning("Permission result: " + permission);
    }

}