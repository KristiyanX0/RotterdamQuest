using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    public List<Item> allRewards = new List<Item>();

    public void AddToInventory(string itemType)
    {   
        try {
            InventoryManager.instance.AddItemToInventory(
                allRewards.Find(item => item.itemName == itemType)
            );
        } catch (NotImplementedException e) {
            Debug.Log(e);
        }
    }
}
