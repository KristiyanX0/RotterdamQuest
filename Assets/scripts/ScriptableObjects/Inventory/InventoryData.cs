using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryData", menuName = "Inventory/Inventory Data")]
public class InventoryData : ScriptableObject
{
    public List<Item> inventory = new List<Item>(); // Stores collected items

    public void AddItem(List<Item> items)
    {
        foreach (Item item in items) {
            inventory.Add(item);
        }
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
    }

    public void RemoveItem(Item item)
    {
        inventory.Remove(item);
    }
}
