using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Land;

[System.Serializable]
public  struct LandSaveState 
{
    public Land.landstatus landStatus;
    public gameTimestamp lastWatered;

    public LandSaveState(Land.landstatus landStatus, gameTimestamp lastWatered)
    {
        this.landStatus = landStatus;
        this.lastWatered = lastWatered;
    }

    public void ClockUpdate(gameTimestamp timestamp)
    {

        //Checked if 24 hours has passed since last watered
        if (landStatus == Land.landstatus.watered)
        {
            //Hours since the land was watered
            int hoursElapsed = gameTimestamp.CompareTimestamps(lastWatered, timestamp);
            Debug.Log(hoursElapsed + " hours since this was watered");


            if (hoursElapsed > 24)
            {
                //Dry up (Switch back to farmland)
                landStatus = Land.landstatus.farmland;
            }
        }

    }

}
