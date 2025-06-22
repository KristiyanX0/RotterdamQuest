using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject gameShield;
    [SerializeField] public GameObject sword;
    InventoryData inventoryData;
    public static BattleManager Instance { get; private set; }
    [SerializeField] private HearthHealthManager PlayerHealth;
    [SerializeField] private HearthHealthManager EnemyHealth;
    [SerializeField] private HealthSerializableConst healthSerializableConst;

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this object.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign this object as the instance and make it persist across scenes.
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void EquipShield(Shield shield)
    {
        PlayerHealth.SetShieldDefense(shield.defensePower);
        gameShield.SetActive(!gameShield.activeSelf);
    }
    void Update()
    {
    }

    private IEnumerator checkDefence()
    {
        // yield return new WaitUntil(() => !gameShield.GetComponent<ObjectToggle>().isHidden);

        PlayerHealth.SetShieldDefense(healthSerializableConst.shieldDefense);
        yield return new WaitForSeconds(1f);
        PlayerHealth.SetShieldDefense(0f);
        gameShield.GetComponent<ObjectToggle>().Hide();

    }

    public void StartCheckDefence()
    {
        if (!gameShield.GetComponent<ObjectToggle>().isHidden)
        {
            StartCoroutine(checkDefence());
        }
    }


    public void HealPlayer(float healAmount)
    {
        PlayerHealth.Heal(healAmount);
    }

    public void EquipWeapon(Weapon weapon)
    {
        sword.SetActive(true);
    }
}
