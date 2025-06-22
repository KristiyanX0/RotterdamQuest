using UnityEngine;

public class Equip : MonoBehaviour
{

    public void EquipShield(HearthHealthManager healthManager, 
                      HealthSerializableConst healthSerializableConst,
                      GameObject target_object) {
        healthManager.SetShieldDefense(healthSerializableConst.shieldDefense);
        target_object.SetActive(!target_object.activeSelf);
    }

}