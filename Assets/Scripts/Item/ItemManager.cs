using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static Dictionary<ConsumableItemCode, int> consumalbeDic = new Dictionary<ConsumableItemCode, int>();
    public Item[] items;

    public Helmet helmet {
        get
        {
            return helmet;
        }
        set
        {
            helmet = value;
        }
    }
    public Armor armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
        }
    }
    public Gauntlet gauntlet
    {
        get
        {
            return gauntlet;
        }
        set
        {
            gauntlet = value;
        }
    }
    public Boots boots {
        get
        {
            return boots;
        }
        set
        {
            boots = value;
        }
    }
    public Shield shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
        }
    }
    public Weapon weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }
    }

    public int sumOfPowerFromEquipments()
    {
        int sum = 0;
        if (helmet != null)
            sum += helmet.power;
        if (armor != null)
            sum += armor.power;
        if (gauntlet != null)
            sum += gauntlet.power;
        if (boots != null)
            sum += boots.power;
        if (shield != null)
            sum += shield.power;
        if (weapon != null)
            sum += weapon.power;
        return sum;
    }

    private void Awake()
    {
        items = FindObjectsOfType<Item>();
    }

}
