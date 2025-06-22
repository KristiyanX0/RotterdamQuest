using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Inventory/Potion")]
public class Potion : Item
{
    public int healAmount;

    public override void Use()
    {
        Debug.Log("Drank potion, healing for " + healAmount);
        BattleManager.Instance.HealPlayer(healAmount);
    }
}
