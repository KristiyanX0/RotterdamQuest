using UnityEngine;

public class InteractionUpdater : MonoBehaviour
{
    void Update()
    {
        GlobalInteractionManager.ProcessInput();
    }
}
