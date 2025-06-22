using RotterdamQuestGameUtils;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSePressedUI : MonoBehaviour
{
    public bool isPressed = false;
    public Sprite pressedSprite;
    public Sprite unpressedSprite;

    public void OnButtonClicked()
    {
        gameObject.GetComponent<Image>().sprite = isPressed ? unpressedSprite : pressedSprite;
        isPressed = !isPressed;
    }
}
