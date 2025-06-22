using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    [SerializeField] private KeyCode exitKey = KeyCode.Escape;
    [SerializeField] private bool useKeyToExit = true;
    
    public static Button exitButton;

    private void Start()
    {
        exitButton = GetComponent<Button>();
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitApplication);
        }
        else
        {
            Debug.LogWarning("Exit button not assigned to ExitGame script");
        }
    }

    private void OnDestroy()
    {
        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(ExitApplication);
        }
    }

    private void Update()
    {
        if (useKeyToExit && Input.GetKeyDown(exitKey))
        {
            ExitApplication();
        }
    }

    public static void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Exiting application");
    }
}