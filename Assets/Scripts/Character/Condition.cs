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

    public Condition(ConditionType type, int turns)
    {
        this.type = type;
        remainedTurns = turns;
    }

    public bool isConditionEnded()
    {
        return remainedTurns <= 0;
    }
}