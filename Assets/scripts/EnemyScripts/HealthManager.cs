using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
    public float shieldDefense = 0f; // Shield defense value

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(damage - shieldDefense, 0); // Reduce damage based on shield
        healthAmount -= finalDamage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0f, 100f);
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void SetShieldDefense(float defense)
    {
        shieldDefense = defense; // Update defense when equipping a shield
    }
}
