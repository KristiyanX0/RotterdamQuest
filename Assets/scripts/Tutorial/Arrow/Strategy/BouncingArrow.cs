using UnityEngine;

public class BouncingArrow : IArrowMovementStrategy
{
    private const float MOVE_SPEED = 20f;
    private const float MOVE_TIME = 0.5f;

    public void StartMove(RectTransform arrow, float amount, float duration)
    {
        if (amount == 0f) amount = MOVE_SPEED;
        if (duration == 0f) duration = MOVE_TIME;
        // Calculate movement direction based on arrow's local Y-axis
        Vector3 moveDirection = arrow.transform.up * amount;

        // Apply movement using LeanTween with proper rotation handling
        LeanTween.moveLocal(arrow.gameObject, arrow.localPosition + moveDirection, duration)
                 .setLoopPingPong();
    }
}
