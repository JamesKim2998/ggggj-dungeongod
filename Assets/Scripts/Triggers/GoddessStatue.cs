using UnityEngine;

public enum StatueType
{
    HEAL,
    BUFF
}

public class GoddessStatue : Trigger
{
    public Coord coord;
    public StatueType type;
    public Hero hero;
    public int buffDistance; //여신상 버프 범위

    private void Start()
    {
        hero = FindObjectOfType<Hero>();
    }

    public override void Act()
    {
        if (Coord.distance(hero.coord, coord) <= buffDistance)
        {
            switch (type)
            {
                case StatueType.HEAL:
                    hero.HP = Mathf.Min(hero.maxHP, hero.HP + 10);
                    return;
                case StatueType.BUFF:
                    // TODO : Hero Condition => RAGE
                    return;
            }
        }
    }
}