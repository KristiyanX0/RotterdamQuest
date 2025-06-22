using RotterdamQuestGameUtils;

public class ArrowMovementFactory
{
    public static IArrowMovementStrategy GetStrategy(TutorialData.ArrowMovementType type)
    {
        switch (type)
        {
            case TutorialData.ArrowMovementType.Bouncing:
                return new BouncingArrow();
            case TutorialData.ArrowMovementType.Sliding:
                return new SlidingArrow();
            case TutorialData.ArrowMovementType.Pulsing:
                return new PulsingArrow();
            default:
                return new BouncingArrow();
        }
    }
}
