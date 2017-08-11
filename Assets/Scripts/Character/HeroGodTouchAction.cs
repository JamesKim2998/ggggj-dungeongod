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
        hero.condition = new Condition(ConditionType.PARALYZED, Random.Range(1, 3));
        hero.HP--;
    }

    public bool IsHeroParalyzed()
    {
        //TODO
        return false; 
    }
}