using UnityEngine;
using UnityEngine.UI;

public class ImageResizer
{
    /// <summary>
    /// Resizes the Image GameObject to fit the new Sprite while anchoring the lower-right corner.
    /// </summary>
    /// <param name="imageGameObject">The Image GameObject to be resized.</param>
    /// <param name="newSprite">The new Sprite to set on the Image.</param>
    public static void AdjustImageToSprite(Image imageGameObject, Sprite newSprite)
    {
        if (imageGameObject == null || newSprite == null)
        {
            Debug.LogError("ImageResizer: Image GameObject or Sprite is null.");
            return;
        }

        RectTransform rectTransform = imageGameObject.rectTransform;

        // Calculate the lower-right anchor point based on the current image
        Vector2 lowerRightCorner = CalculateLowerRightAnchor(rectTransform);

        // Set the new sprite
        imageGameObject.sprite = newSprite;

        // Adjust the size of the RectTransform to match the new sprite's aspect ratio
        float aspectRatio = newSprite.rect.width / newSprite.rect.height;
        float newHeight = newSprite.rect.height; // Keep the height unchanged
        float newWidth = newSprite.rect.width;
        rectTransform.sizeDelta = new Vector2(newWidth, newHeight);

        // Calculate the new lower-right corner after resizing
        Vector2 newLowerRightCorner = CalculateLowerRightAnchor(rectTransform);

        // Calculate the offset to adjust the position
        Vector2 offset = lowerRightCorner - newLowerRightCorner;
        rectTransform.anchoredPosition += offset;
    }

    /// <summary>
    /// Calculates the lower-right anchor point of a RectTransform.
    /// </summary>
    /// <param name="rectTransform">The RectTransform to calculate the anchor point for.</param>
    /// <returns>The lower-right anchor point in local space.</returns>
    private static Vector2 CalculateLowerRightAnchor(RectTransform rectTransform)
    {
        float x = rectTransform.anchoredPosition.x + (rectTransform.sizeDelta.x * (1 - rectTransform.pivot.x));
        float y = rectTransform.anchoredPosition.y - (rectTransform.sizeDelta.y * rectTransform.pivot.y);
        return new Vector2(x, y);
    }
}
