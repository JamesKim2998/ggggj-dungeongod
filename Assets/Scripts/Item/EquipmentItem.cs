public enum EquipmentType
{
    HELMET,
    GAUNTLET,
    ARMOR,
    SHIELD,
    BOOTS,
    WEAPON
}

public class EquipmentItem : Item
{
    public EquipmentType type;
    public int power;
}