using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class HearthHealthManager : MonoBehaviour
{
    private Action<float> updateHealth;
    [SerializeField]
    private HearthHealthBar healthBar;
    public float currentHealth;// { get; private set; } // for debuging purposes I make it public
    private float maxHealthAmount;
    private float singleHit { get; set; }
    private float singleHeal { get; set; }
    private float shieldDefense; // Shield defense value

    [SerializeField]
    private HealthSerializableConst healthSerializableConst;

    void Start()
    {
        maxHealthAmount = healthSerializableConst.maxHealthAmount;
        singleHit = healthSerializableConst.singleHit;
        singleHeal = healthSerializableConst.singleHeal;
        shieldDefense = healthSerializableConst.shieldDefense;
        
        currentHealth = maxHealthAmount;

        updateHealth += healthBar.SetHealth;
    }

    public void TakeDamage(float damage)
    {
        float DamageToBeTaken = Math.Clamp(damage - shieldDefense, 0f, currentHealth);
        setHealth(currentHealth - DamageToBeTaken);
    }

    public void TakeDamage()
    {
        TakeDamage(singleHit);
    }

    public void Heal(float healAmount)
    {
        float HealthToBeAdded = Mathf.Clamp(healAmount, 0f, maxHealthAmount - currentHealth);
        setHealth(currentHealth + HealthToBeAdded);
    }

    public void Heal()
    {
        Heal(singleHeal);
    }

    public void SetShieldDefense(float defense)
    {
        shieldDefense = defense;
    }

    public void RemoveShieldDefense(float defense)
    {
        shieldDefense = defense;
    }

    private void setHealth(float health)
    {
        currentHealth = health;
        updateHealth?.Invoke(currentHealth);
    }
}
