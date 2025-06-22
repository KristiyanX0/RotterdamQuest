using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthConstants", menuName = "Health/Health Data")]
public class HealthSerializableConst : ScriptableObject
{
    public float maxHealthAmount = 100f;
    public float singleHit = 10f;
    public float singleHeal = 10f;
    public float shieldDefense = 0f;
    
    public List<Sprite> hearts = new List<Sprite>();
}
