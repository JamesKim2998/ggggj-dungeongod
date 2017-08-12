using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static Dictionary<ConsumableItemCode, int> consumalbeDic = new Dictionary<ConsumableItemCode, int>();

    public static Dictionary<EquipmentType, EquipmentCode> heroEquipInfo = new Dictionary<EquipmentType, EquipmentCode>();

    public static Dictionary<EquipmentCode, EquipmentInfo> equipDic = new Dictionary<EquipmentCode, EquipmentInfo>();

    public Item[] items;

    public int sumOfPowerFromEquipments()
    {
        return equipDic[heroEquipInfo[EquipmentType.ARMOR]].power + equipDic[heroEquipInfo[EquipmentType.WEAPON]].power;
    }
}
