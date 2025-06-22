using RotterdamQuestGameUtils;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private IArrowMovementStrategy movementStrategy;
    public RectTransform arrow; // Assign in Inspector

    public void Show(TutorialData step)
    {
        if (arrow?.gameObject == null)
        {
            Debug.LogError("Arrow object is null. Cannot display tutorial arrow.");
            return;
        }

        arrow.gameObject.SetActive(true);
        
        // Try to move UI arrow, fallback to world space if necessary
        RectTransform rectTransform = arrow.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = step.targetPosition;
            rectTransform.rotation = Quaternion.Euler(step.targetRotation);
        }
        else
        {
            arrow.transform.SetPositionAndRotation(step.targetPosition, Quaternion.Euler(step.targetRotation));
        }

        // Apply movement strategy if available
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        arrowController.movementStrategy = ArrowMovementFactory.GetStrategy(step.movementType);
        movementStrategy.StartMove(arrow, step.amount, step.duration);
    }


    public void Hide()
    {
        if (arrow?.gameObject == null)
        {
            Debug.LogError("Arrow object is null. Cannot hide.");
            return;
        }
        LeanTween.cancel(arrow.gameObject);
        arrow.gameObject.SetActive(false);
    }

}
