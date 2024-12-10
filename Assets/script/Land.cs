using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public enum landstatus
    {
        soil, farmland, watered
    }

    public landstatus LandStatus;

    public Material soilMat, farmlandMat, wateredMat;
    new Renderer renderer;

    public GameObject select;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        SwitchLandStatus(landstatus.soil);

        Select(false);
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
                break;
        }

        //render
        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    //interact
    public void Interact()
    {
        SwitchLandStatus(landstatus.farmland);
    }

}
