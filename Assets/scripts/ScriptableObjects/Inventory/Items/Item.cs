using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    public enum ItemType { Potion, Weapon, Shield, Other }

    public virtual void Use()
    {
        Debug.Log("Used " + itemName);
    }
}
