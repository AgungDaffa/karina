using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

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

    [Header("Tools")]
    //Tool Slots
    public itemData[] tools = new itemData[8];
    //Tool in the player's hand
    public itemData equippedTool = null;

    [Header("Items")]
    //Item Slots
    public itemData[] items = new itemData[8];
    //Item in the player's hand
    public itemData equippedItem = null;

    //Equipping

    //Handles movement of item from Inventory to Hand
    public void InventoryToHand()
    {

    }

    //Handles movement of item from Hand to Inventory
    public void HandToInventory()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
