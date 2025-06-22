using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HearthHealthBar : MonoBehaviour
{
    [SerializeField]
    private List<Image> hearts;
    [SerializeField]
    private HealthSerializableConst healthSerializableConst;
    
    // This list should be ordered such that:
    // index 0 = full heart sprite,
    // last index = empty heart sprite,
    // with any intermediate sprites representing partial fills.
    private List<Sprite> heartsSprites = new List<Sprite>();    
    private float maxHealthAmount;

    void Start()
    {
        maxHealthAmount = healthSerializableConst.maxHealthAmount;
        
        // Populate heartsSprites from the HealthSerializableConst configuration.
        foreach (Sprite sprite in healthSerializableConst.hearts)
        {
            heartsSprites.Add(sprite);
        }

        // Set initial health display to full health.
        SetHealth(maxHealthAmount);
    }
    
    public void SetHealth(float currentHealth)
    {
        // Clamp the current health between 0 and the max value.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealthAmount);
        
        // Determine how much health each heart represents.
        float healthPerHeart = maxHealthAmount / hearts.Count;

        for (int i = 0; i < hearts.Count; i++)
        {
            // Calculate the fill level for this heart.
            float heartValue = (currentHealth - (i * healthPerHeart)) / healthPerHeart;
            
            // Clamp the fill level between 0 (empty) and 1 (full).
            float heartFill = Mathf.Clamp01(heartValue);
            
            // Map heartFill to a sprite index.
            // When heartFill is 1 (full), we want index 0.
            // When heartFill is 0 (empty), we want the last sprite.
            // The formula below scales (1 - heartFill) to the range [0, heartsSprites.Count).
            int spriteIndex = Mathf.Clamp(
                Mathf.FloorToInt((1 - heartFill) * heartsSprites.Count),
                0,
                heartsSprites.Count - 1
            );
            
            hearts[i].sprite = heartsSprites[spriteIndex];
        }
    }
}
