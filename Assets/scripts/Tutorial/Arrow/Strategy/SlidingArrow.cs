using UnityEngine;

public class SlidingArrow : IArrowMovementStrategy
{
    private const float SLIDE_DISTANCE = 50f;
    private const float SLIDE_TIME = 1f;

    public void StartMove(RectTransform arrow, float amount, float duration)
    {
        if (amount == 0f) amount = SLIDE_DISTANCE;
        if (duration == 0f) duration = SLIDE_TIME;
        arrow.LeanMoveLocalY(arrow.localPosition.y - amount, duration).setLoopClamp();
    }
}
