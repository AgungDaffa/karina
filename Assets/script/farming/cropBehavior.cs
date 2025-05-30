using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cropBehavior : MonoBehaviour
{
    //The ID of the land the crop belongs to
    int landID;

    //Information on what the crop will grow into 
    seedData seedToGrow;

    [Header("Stages of Life")]
    public GameObject seed;
    public GameObject wilted;
    private GameObject seedling;
    private GameObject harvestable;

    //The growth points of the crop
    int growth;
    //How many growth points it takes before it becomes harvestable
    int maxGrowth;

    //The crop can stay alive for 48 hours without water before it dies
    int maxHealth = gameTimestamp.HoursToMinutes(48);

    int health;

    public enum CropState
    {
        Seed, Seedling, Harvestable, Wilted
    }
    //The current stage in the crop's growth
    public CropState cropState;

    //Initialisation for the crop GameObject
    //Called when the player plants a seed
    public void Plant(int landID, seedData seedToGrow)
    {
        LoadCrop(landID, seedToGrow, CropState.Seed, 0, 0);
        LandManager.Instance.RegisterCrop(landID, seedToGrow, cropState, growth, health);

    }
    public void LoadCrop(int landID, seedData seedToGrow, CropState cropState, int growth, int health)
    {
        this.landID = landID;
        //Save the seed information
        this.seedToGrow = seedToGrow;

        //Instantiate the seedling and harvestable GameObjects
        seedling = Instantiate(seedToGrow.seedling, transform);

        //Access the crop item data
        ItemData cropToYield = seedToGrow.cropToYield;

        //Instantiate the harvestable crop
        harvestable = Instantiate(cropToYield.gameModel, transform);

        //Convert Days To Grow into hours
        int hoursToGrow = gameTimestamp.DaysToHours(seedToGrow.daysToGrow);
        //Convert it to minutes
        maxGrowth = gameTimestamp.HoursToMinutes(hoursToGrow);

        //Set the growth and health accordingly
        this.growth = growth;
        this.health = health;

        //Check if it is regrowable
        if (seedToGrow.regrowable)
        {
            //Get the RegrowableHarvestBehaviour from the GameObject
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();

            //Initialise the harvestable 
            regrowableHarvest.SetParent(this);
        }

        //Set the initial state to Seed
        SwitchState(cropState);

    }
    //The crop will grow when watered
    public void Grow()
    {
        //Increase the growth point by 1
        growth++;

        //Restore the health of the plant when it is watered
        if (health < maxHealth)
        {
            health++;
        }

        //The seed will sprout into a seedling when the growth is at 50%
        if (growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        //Grow from seedling to harvestable
        if (growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }
        //Inform LandManager on the changes
        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }

    //Function to handle the state changes 
    void SwitchState(CropState stateToSwitch)
    {
        //Reset everything and set all GameObjects to inactive
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);


        switch (stateToSwitch)
        {
            case CropState.Seed:
                //Enable the Seed GameObject
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                //Enable the Seedling GameObject
                seedling.SetActive(true);

                //Give the seed health 
                health = maxHealth;
                break;
            case CropState.Harvestable:
                //Enable the Harvestable GameObject
                harvestable.SetActive(true);
                //If the seed is not regrowable, detach the harvestable from this crop gameobject and destroy it. 
                if (!seedToGrow.regrowable)
                {
                    //Unparent it to the crop
                    harvestable.transform.parent = null;
                    RemoveCrop();
                }

                break;

            case CropState.Wilted:
                //Enable the wilted GameObject
                wilted.SetActive(true);
                break;
        }

        //Set the current crop state to the state we're switching to
        cropState = stateToSwitch;
    }

    //Destroys and Deregisters the Crop
    public void RemoveCrop()
    {
        LandManager.Instance.DeregisterCrop(landID);
        Destroy(gameObject);
    }

    //Called when the player harvests a regrowable crop. Resets the state to seedling 
    public void Regrow()
    {
        //Reset the growth 
        //Get the regrowth time in hours
        int hoursToRegrow = gameTimestamp.DaysToHours(seedToGrow.daysToRegrow);
        growth = maxGrowth - gameTimestamp.HoursToMinutes(hoursToRegrow);

        //Switch the state back to seedling
        SwitchState(CropState.Seedling);
    }

    //The crop will progressively wither when the soil is dry 
    public void Wither()
    {
        health--;
        //If the health is below 0 and the crop has germinated, kill it
        if (health <= 0 && cropState != CropState.Seed)
        {
            SwitchState(CropState.Wilted);
        }
        //Inform LandManager on the changes
        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }


}
