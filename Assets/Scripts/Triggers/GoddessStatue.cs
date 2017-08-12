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
    public int buffDistance; //여신상 버프 범위
    public int healAmount;

    private void Awake()
    {
        coord = Coord.Round(transform.localPosition);
    }

    public override void Act()
    {
        Hero hero = MainLogic.instance.hero;
        if (Coord.distance(hero.coord, coord) <= buffDistance)
        {
            Debug.Log("Goddess Statue Activated");
            switch (type)
            {
                case StatueType.HEAL:
                    hero.HP = Mathf.Min(hero.maxHP, hero.HP + healAmount);
                    return;
                case StatueType.BUFF:
                    hero.buffed = true;
                    return;
            }
        }
    }
}