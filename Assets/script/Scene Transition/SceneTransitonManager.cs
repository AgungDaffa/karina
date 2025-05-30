using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitonManager : MonoBehaviour
{
    public static SceneTransitonManager Instance;

    //The scenes the player can enter
    public enum Location { Ranch, PlayerHome, Town }
    public Location currentLocation;
    //The player's transform
    Transform playerPoint;

    //Check if the screen has finished fading out
    bool screenFadedOut;

    private void Awake()
    {
        //If there is more than 1 instance, destroy GameObject
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }

        //Make the gameobject persistent across scenes
        DontDestroyOnLoad(gameObject);
        //OnLocationLoad will be called when the scene is loaded
        SceneManager.sceneLoaded += OnLocationLoad;

        //Find the player's transform
        playerPoint = FindObjectOfType<PlayerController>().transform;
    }
    //Switch the player to another scene
    public void SwitchLocation(Location locationToSwitch)
    {
        UIManager.Instance.FadeOutScreen();
        screenFadedOut = false;
        StartCoroutine(ChangeScene(locationToSwitch));
    }

    IEnumerator ChangeScene(Location locationToSwitch)
    {
        //Disable the player's CharacterController component
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;
        //Wait for the scene to finish fading out before loading the next scene
        while (!screenFadedOut)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //Reset the boolean
        screenFadedOut = false;
        UIManager.Instance.ResetFadeDefaults();
        SceneManager.LoadScene(locationToSwitch.ToString());

    }

    //Called when the screen has faded out
    public void OnFadeOutComplete()
    {
        screenFadedOut = true;

    }

    //Called when a scene is loaded
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        //The location the player is coming from when the scene loads
        Location oldLocation = currentLocation;

        //Get the new location by converting the string of our current scene into a Location enum value
        Location newLocation = (Location)Enum.Parse(typeof(Location), scene.name);

        //If the player is not coming from any new place, stop executing the function
        if (currentLocation == newLocation) return;

        //Find the start point
        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);

        //Disable the player's CharacterController component
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        //Change the player's position to the start poin
        playerPoint.position = startPoint.position;
        playerPoint.rotation = startPoint.rotation;

        //Re-enable player character controller so he can move
        playerCharacter.enabled = true;

        //Save the current location that we just switched to
        currentLocation = newLocation;
    }
}