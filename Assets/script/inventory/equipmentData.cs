using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Equipment")]
public class equipmentData : itemData
{
    public enum ToolType
    {
        Hoe, WateringCan, Axe, Pickaxe
    }
    public ToolType toolType;
}
