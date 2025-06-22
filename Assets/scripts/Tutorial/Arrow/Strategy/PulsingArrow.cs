using UnityEngine;

public class PulsingArrow : IArrowMovementStrategy
{
    private const float PULSE_SCALE = 1.2f;
    private const float PULSE_DURATION = 0.5f;

    public void StartMove(RectTransform arrow, float amount, float duration)
    {
        if (amount == 0f) amount = PULSE_SCALE;
        if (duration == 0f) duration = PULSE_DURATION;
        arrow.LeanScale(Vector3.one * amount, duration).setLoopPingPong();
    }
}
