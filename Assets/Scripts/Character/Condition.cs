public enum ConditionType
{
    EXPLORE,
    WAIT,
    GATHER,
    COMBAT,
    RUNAWAY,
    PARALYZED,
    PANIC
}

public class Condition
{
    public ConditionType type;
    public int remainedTurns;

    public void checkConditionEnded()
    {
        if (remainedTurns <= 0)
        {
            //TODO
        }
    }
}