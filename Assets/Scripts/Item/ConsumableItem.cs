using System.Collections.Generic;

public enum ConsumableItemCode
{
    CAKE,
    JEON,
    POTION
}

public class ConsumableItem
{
    public virtual void  use()
    {

    }
}

public class HealingItem : ConsumableItem
{
    public int healAmount;
    public override void use()
    {
        MainLogic.instance.hero.HP += healAmount;
    }
}

public class BuffItem : ConsumableItem
{
    int turn;
    public override void use()
    {
        MainLogic.instance.hero.buffedTurn += turn;
    }
}