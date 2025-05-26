using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Land;

public class Land : MonoBehaviour, iTimeTracker
{
    public int id;
    public enum landstatus
    {
        soil, farmland, watered
    }

    public landstatus LandStatus;

    public Material soilMat, farmlandMat, wateredMat;
    new Renderer renderer;

    public GameObject select;

    gameTimestamp timeWatered;

    [Header("Crops")]
    //The crop prefab to instantiate
    public GameObject cropPrefab;

    //The crop currently planted on the land
    cropBehavior cropPlanted = null;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        SwitchLandStatus(landstatus.soil);

        Select(false);

        //Add this to TimeManager's Listener list
        timeManager.Instance.RegisterTracker(this);

    }

    public void LoadLandData(landstatus statusToSwitch, gameTimestamp lastWatered)
    {
        //Set land status accordingly
        LandStatus = statusToSwitch;
        timeWatered = lastWatered;

        Material materialToSwitch = soilMat;

        //Decide what material to switch to
        switch (statusToSwitch)
        {
            case landstatus.soil:
                //Switch to the soil material
                materialToSwitch = soilMat;
                break;
            case landstatus.farmland:
                //Switch to farmland material 
                materialToSwitch = farmlandMat;
                break;

            case landstatus.watered:
                //Switch to watered material
                materialToSwitch = wateredMat;
                break;

        }

        //Get the renderer to apply the changes
        renderer.material = materialToSwitch;
    }

    public void SwitchLandStatus(landstatus statusToSwitch)
    {
        LandStatus = statusToSwitch;

        Material materialToSwitch = soilMat;
        
        switch(statusToSwitch)
        {
            case landstatus.soil:
                materialToSwitch = soilMat;
                break; 
            case landstatus.farmland:
                materialToSwitch = farmlandMat;
                break;
            case landstatus.watered:
                materialToSwitch = wateredMat;
                //Cache the time it was watered
                timeWatered = timeManager.Instance.GetGameTimestamp();
                break;
        }

        //render
        renderer.material = materialToSwitch;

        LandManager.Instance.OnLandStateChange(id, LandStatus, timeWatered);
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    //interact
    public void Interact()
    {
        //Check the player's tool slot
        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        //If there's nothing equipped, return
        if (!InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Tool))
        {
            return;
        }

        //Try casting the itemdata in the toolslot as EquipmentData
        equipmentData equipmentTool = toolSlot as equipmentData;

        //Check if it is of type EquipmentData 
        if (equipmentTool != null)
        {
            //Get the tool type
            equipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case equipmentData.ToolType.Hoe:
                    SwitchLandStatus(landstatus.farmland);
                    break;
                case equipmentData.ToolType.WateringCan:
                    //The land must be tilled first
                    if (LandStatus != landstatus.soil)
                    {
                        SwitchLandStatus(landstatus.watered);
                    }
                    break;

                case equipmentData.ToolType.shovel:

                    //Remove the crop from the land
                    if (cropPlanted != null)
                    {
                        cropPlanted.RemoveCrop();
                    }
                    break;
            }

            return;

        }

        //Try casting the itemdata in the toolslot as SeedData
        seedData seedTool = toolSlot as seedData;

        ///Conditions for the player to be able to plant a seed
        ///1: He is holding a tool of type SeedData
        ///2: The Land State must be either watered or farmland
        ///3. There isn't already a crop that has been planted
        if (seedTool != null && LandStatus != landstatus.soil && cropPlanted == null)
        {
            SpawnCrop();
            //Plant it with the seed's information
            cropPlanted.Plant(id, seedTool);

            //Consume the item
            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));

        }

    }
    public cropBehavior SpawnCrop()
    {
        //Instantiate the crop object parented to the land
        GameObject cropObject = Instantiate(cropPrefab, transform);
        //Move the crop object to the top of the land gameobject
        cropObject.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        //Access the CropBehaviour of the crop we're going to plant
        cropPlanted = cropObject.GetComponent<cropBehavior>();
        return cropPlanted;
    }
    public void ClockUpdate(gameTimestamp timestamp)
    {
        //Checked if 24 hours has passed since last watered
        if (LandStatus == landstatus.watered)
        {
            //Hours since the land was watered
            int hoursElapsed = gameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log(hoursElapsed + " hours since this was watered");

            //Grow the planted crop, if any
            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }


            if (hoursElapsed > 24)
            {
                //Dry up (Switch back to farmland)
                SwitchLandStatus(landstatus.farmland);
            }
        }

        //Handle the wilting of the plant when the land is not watered
        if (LandStatus != landstatus.watered && cropPlanted != null)
        {
            //If the crop has already germinated, start the withering
            if (cropPlanted.cropState != cropBehavior.CropState.Seed)
            {
                cropPlanted.Wither();
            }
        }
    }
    private void OnDestroy()
    {
        timeManager.Instance.UnregisterTracker(this);
    }
}
