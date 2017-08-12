using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static Dictionary<ConsumableItemCode, int> consumalbeDic = new Dictionary<ConsumableItemCode, int>();
    public static Dictionary<EquipmentType, int> equipDic = new Dictionary<EquipmentType, int>();
    public Item[] items;

    public int sumOfPowerFromEquipments()
    {
        int sum = 0;
        for (int i=0; i<=6; ++i)
        {
            sum += equipDic[(EquipmentType)i];
        }
        return sum;
    }

    private void Awake()
    {
        items = FindObjectsOfType<Item>();
    }

}
