using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static cropBehavior;

[System.Serializable]
public struct CropSaveState 
{
    //The index of the land the crop is planted on 
    public int landID;

    public string seedToGrow;
    public cropBehavior.CropState cropState;
    public int growth;
    public int health;

    public CropSaveState(int landID, string seedToGrow, cropBehavior.CropState cropState, int growth, int health)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        this.health = health;
    }
    public void Grow()
    {
        //Increase the growth point by 1
        growth++;

        seedData seedInfo = (seedData)InventoryManager.Instance.itemIndex.GetItemFromString(seedToGrow);

        int maxGrowth = gameTimestamp.HoursToMinutes(gameTimestamp.DaysToHours(seedInfo.daysToGrow));
        int maxHealth = gameTimestamp.HoursToMinutes(48);

        //Restore the health of the plant when it is watered
        if (health < maxHealth)
        {
            health++;
        }

        //The seed will sprout into a seedling when the growth is at 50%
        if (growth >= maxGrowth / 2 && cropState == cropBehavior.CropState.Seed)
        {
           cropState = cropBehavior.CropState.Seedling;
        }

        //Grow from seedling to harvestable
        if (growth >= maxGrowth && cropState == cropBehavior.CropState.Seedling)
        {
            cropState = cropBehavior.CropState.Harvestable;
        }
    }
    public void Wither()
    {
        health--;
        //If the health is below 0 and the crop has germinated, kill it
        if (health <= 0 && cropState != cropBehavior.CropState.Seed)
        {
            cropState = cropBehavior.CropState.Wilted;
        }
    }

}
