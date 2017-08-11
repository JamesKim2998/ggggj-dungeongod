public class TriggerGodTouchAction : GodTouchAction
{
    public Trigger receiverTrigger;

    public override void Act()
    {
        receiverTrigger.Act();
    }
}
