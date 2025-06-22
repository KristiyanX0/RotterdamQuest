using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private static bool settingsInstanceExists = false;

    void Awake()
    {
        if (settingsInstanceExists)
        {
            Destroy(gameObject);
            return;
        }

        
        settingsInstanceExists = true;
        DontDestroyOnLoad(gameObject);
    }
}