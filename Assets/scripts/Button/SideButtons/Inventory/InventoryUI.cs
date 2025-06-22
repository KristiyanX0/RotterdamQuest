using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    [SerializeField]
    private RectTransform contentPanel;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject itemSlotPrefab;

    public void UpdateInventoryUI(Item item)
    {
        AddItemUIInventory(item);
        RefreshLayout();
        RefreshText();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {

    }

    private void AddItemUIInventory(Item item)
    {
        if (item == null) return;
        GameObject slot = Instantiate(itemSlotPrefab, contentPanel); // Create new UI element
        slot.GetComponentInChildren<InventoryItem>().Intialize(item);
        slot.name = item.itemName; // Naming for debugging
    }

    
    public void RemoveItemUIInventory(Item item)
    {
        foreach (Transform child in contentPanel)
        {
            if (child.name == item.itemName)
            {
                Destroy(child.gameObject);
                RefreshLayout();
                RefreshText();
                return;
            }
        }
    }

    public void SyncInventory(List<Item> allInventoryData) {
        foreach (Transform child in contentPanel) {
            Destroy(child.gameObject);
        }
        foreach (var item in allInventoryData) {
            AddItemUIInventory(item);
        }
        // UpdateInventoryUI(null);
    }

    public void RefreshLayout()
    {
        var contentRect = contentPanel.GetComponent<RectTransform>();
        var neededWidth = contentPanel.GetChild(0).GetComponent<RectTransform>().sizeDelta.x * contentPanel.childCount + 
            contentPanel.GetComponent<HorizontalLayoutGroup>().spacing * (contentPanel.childCount - 1);
        if (neededWidth > contentRect.sizeDelta.x)
        {
            contentRect.sizeDelta = new Vector2(neededWidth, contentRect.sizeDelta.y);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);
    }
    
    public void RefreshText()
    {
        text.text = contentPanel.childCount.ToString() + " ITEMS";
    }

    public void HighlightEquippedItem(Item item)
    {
        // Implement logic to highlight equipped item visually
        Debug.Log(item.itemName + " is equipped!");
    }
}
