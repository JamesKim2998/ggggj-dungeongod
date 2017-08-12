using System.Collections.Generic;
using UnityEngine;

public enum ConsumableItemCode
{
    CAKE,
    CHICKEN,
    PIE,
    WINE,
    SPAGETTI,
    EGGJJIM    
}

public class ConsumableItem : Item
{
    public ConsumableItemCode code;
}