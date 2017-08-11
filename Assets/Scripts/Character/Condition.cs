public enum ConditionType
{
    NORMAL,
    PANIC,
    PARALYZED,
    RAGE
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