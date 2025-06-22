using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CandyCoded.HapticFeedback;

[DefaultExecutionOrder(-100)]  // ensure this runs early
public class ButtonVibrationManager : MonoBehaviour
{
    private static ButtonVibrationManager _instance;
    private readonly HashSet<Button> _subscribedButtons = new HashSet<Button>();

    void Awake()
    {
        // Singleton pattern
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe on start and on every new scene load
        SceneManager.sceneLoaded += OnSceneLoaded;
        SubscribeAllButtons();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SubscribeAllButtons();
    }

    private void SubscribeAllButtons()
    {
#if UNITY_ANDROID
        // Find all Buttons in the active scene
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (var btn in buttons)
        {
            if (_subscribedButtons.Add(btn))
            {
                btn.onClick.AddListener(HandleVibration);
            }
        }
#endif
    }

    private void HandleVibration()
    {
        HapticFeedback.MediumFeedback();
    }

    void OnDestroy()
    {
        // Clean up event subscription
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
