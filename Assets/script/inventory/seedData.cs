using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Items/Seed")]
public class seedData : itemData
{
    //Time it takes before the seed matures into a crop
    public int daysToGrow;

    public itemData cropToYield;
}
