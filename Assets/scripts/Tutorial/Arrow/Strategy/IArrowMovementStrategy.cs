using UnityEngine;

public interface IArrowMovementStrategy
{
    void StartMove(RectTransform arrow, float amount = 0f, float duration = 0f);
}
