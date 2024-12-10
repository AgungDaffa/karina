using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;

    Land selectedLand =  null;

    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();    
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
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

        if(selectedLand != null)
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
        if(selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }
        Debug.Log("Non on ant land");
    }

}
