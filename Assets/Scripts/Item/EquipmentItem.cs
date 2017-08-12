public enum EquipmentType
{
    ARMOR,
    WEAPON
}

public enum EquipmentCode
{
    NONE,
    ARMOR0,
    ARMOR1,
    ARMOR2,
    ARMOR3,
    WEAPON0,
    WEAPON1,
    WEAPON2,
    WEAPON3
}

public struct EquipmentInfo
{
    public EquipmentType type;
    public int power;

    public EquipmentInfo(EquipmentType t, int p)
    {
        type = t;
        power = p;
    }
}


public class EquipmentItem : Item
{
    public EquipmentCode code;
}