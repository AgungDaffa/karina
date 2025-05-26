using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;

    Land selectedLand =  null;

    //The interactable object the player is currently selecting
    InteractableObject selectedInteractable = null;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogWarning("PlayerController tidak ditemukan di parent hierarchy!");
        }
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {

            print(hit.collider.name);

            OnInteractableHit(hit);
        }
    }

    //interact hit
    void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;
        
        if(other.tag == "Land")
        {
            Land land = other.GetComponent<Land>();
            SelectedLand(land);
            return;
        }

        //Check if the player is going to interact with an Item
        if (other.tag == "Item")
        {
            //Set the interactable to the currently selected interactable
            selectedInteractable = other.GetComponent<InteractableObject>();
            return;
        }

        //Deselect the interactable if the player is not standing on anything at the moment
        if (selectedInteractable != null)
        {
            selectedInteractable = null;
        }

        if (selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }
    }

    void SelectedLand(Land land)
    {
        if(selectedLand != null)
        {
            selectedLand.Select(false);
        }

        selectedLand = land;
        land.Select(true);
    }   

    public void Interact()
    {
        //The player shouldn't be able to use his tool when he has his hands full with an item
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            return;
        }

        if (selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }
        Debug.Log("Non on ant land");


    }

    //Triggered when the player presses the item interact button
    public void ItemInteract()
    {
        //If the player is holding something, keep it in his inventory
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
            return;
        }

    
        //If the player isn't holding anything, pick up an item
        //Check if there is an interactable selected
        if (selectedInteractable != null)
        {
            //Pick it up
            selectedInteractable.Pickup();
        }
    }

}
