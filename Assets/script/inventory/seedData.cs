using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Items/Seed")]
public class seedData : ItemData
{
    //Time it takes before the seed matures into a crop
    public int daysToGrow;

    public ItemData cropToYield;

    //The seedling GameObject
    public GameObject seedling;

    [Header("Regrowable")]
    //Is the plant able to regrow the crop after being harvested?
    public bool regrowable;
    //Time taken before the plant yields another crop
    public int daysToRegrow;

}
