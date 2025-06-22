using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShield", menuName = "Inventory/Shield")]
public class Shield : Item
{
    public int defensePower;
    

    public override void Use()
    {
        Debug.Log("Equipped " + itemName + " with " + defensePower + " defense power.");
        BattleManager.Instance.EquipShield(this);
    }
}
