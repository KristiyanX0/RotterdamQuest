using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/Weapon")]
public class Weapon : Item
{
    public int attackPower;

    public override void Use()
    {
        Debug.Log("Equipped " + itemName + " with " + attackPower + " attack power.");
        BattleManager.Instance.EquipWeapon(this);
    }
}

