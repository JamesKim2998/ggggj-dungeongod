using UnityEngine;

public enum StatueType
{
    HEAL,
    BUFF
}

public class GoddessStatue : Trigger
{
    public StatueType type;
    public int buffDistance; //여신상 버프 범위
    public int healAmount;

    public override void Act()
    {
        var hero = MainLogic.instance.hero;
        var coord = Coord.Round(transform.position);
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
        StartCoroutine(AudioManager.playSFX(Camera.main.gameObject.AddComponent<AudioSource>(), MainLogic.instance.audioManager.SFXs[8]));
    }
}