using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    public List<SceneMusic> sceneMusicList;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on this GameObject. Please add one.");
            enabled = false;
            return;
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() // on enabling object 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() // on disabling object
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        foreach (SceneMusic entry in sceneMusicList)
        {
            if (entry.sceneName == sceneName)
            {
                if (audioSource.clip != entry.musicClip)
                {
                    audioSource.clip = entry.musicClip;
                    audioSource.Play();
                }
                return;
            }
        }

        audioSource.Stop();
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    public void ResumeMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.UnPause();
    }

    public void TogglePause()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
        else
            audioSource.UnPause();
    }

    public void ToggleMute()
    {
        audioSource.mute = !audioSource.mute;
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }

    public void ChangeTrack(AudioClip newClip)
    {
        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }

}
