using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour, iTimeTracker
{
    public static GameStateManager Instance { get; private set; }

    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timeManager.Instance.RegisterTracker(this);
    }
    public void ClockUpdate(gameTimestamp timestamp)
    {
        if(SceneTransitonManager.Instance.currentLocation != SceneTransitonManager.Location.Ranch)
        {
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> CropData = LandManager.farmData.Item2;

            //no crop
            if (CropData.Count == 0) return;

            for (int i = 0; i < CropData.Count; i++)
            {
                CropSaveState crop = CropData[i];
                LandSaveState land = landData[crop.landID];
            }
        }
    }

}
