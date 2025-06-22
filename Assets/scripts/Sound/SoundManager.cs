using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;
    public static float volume = 1.0f;
    public static bool isSoundEnabled = true;
    public bool isVFXEnabled = isSoundEnabled;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        isVFXEnabled = isSoundEnabled;
    }

    public void PlaySound(AudioClip clip)
    {
        if (!isSoundEnabled || clip == null) return;

        audioSource.PlayOneShot(clip, volume);
    }

    public void ToggleSound()
    {
        Debug.Log("Toggling sound. Current state: " + (isSoundEnabled ? "Enabled" : "Disabled"));
        isSoundEnabled = !isSoundEnabled;
        audioSource.mute = !isSoundEnabled;
    }
}
