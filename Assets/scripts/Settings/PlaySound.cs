using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlaySound : MonoBehaviour
{
    public AudioClip soundClip;

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(play);
        }
    }

    void play()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(soundClip);
        }
    }
}
