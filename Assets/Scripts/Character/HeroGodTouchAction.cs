using UnityEngine;

public class HeroGodTouchAction : GodTouchAction
{
    public Hero hero;
    private void Start()
    {
        hero = GetComponent<Hero>();
    }
    public override void Act()
    {
        // TODO : Hero 기절, HP 1 감소
        hero.condition = ConditionType.PARALYZED;
        hero.HP--;
    }

    public bool IsHeroParalyzed()
    {
        //TODO
        return false; 
    }
}