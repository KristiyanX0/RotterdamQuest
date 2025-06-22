using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private Image image;
    public Item item;

    public void Use() {
        item.Use();
    }

    public void Intialize(Item item) {
        if (item == null) {
            Debug.Log("Item is null");
            return;
        }
        this.item = item;
        image = GetComponent<Image>();
        if (image == null) {
            Debug.Log("Image is null");
            return;
        }
        image.sprite = item.icon;
    }

}
