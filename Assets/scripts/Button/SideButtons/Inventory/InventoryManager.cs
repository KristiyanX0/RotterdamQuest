using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryData inventoryData;
    public InventoryUI inventoryUI;
    public Action<Item> AddItemToInventory;
    public Action<List<Item>> syncInventory;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        AddItemToInventory += inventoryData.AddItem;
        AddItemToInventory += inventoryUI.UpdateInventoryUI;

        AddItemToInventory += SyncInventory;
        SyncInventory(null);
    }

    void SyncInventory(Item item)
    {
        // item should not be used here
        inventoryUI.SyncInventory(inventoryData.inventory);
    }
}
