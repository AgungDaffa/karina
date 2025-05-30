using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeManager : MonoBehaviour
{
    public static timeManager Instance { get; private set; }

    [Header("Internal Clock")]
    [SerializeField]
    gameTimestamp timestamp;
    public float timeScale = 1.0f;

    [Header("Day and Night cycle")]
    //The transform of the directional light (sun)
    public Transform sunTransform;

    //List of Objects to inform of changes to the time
    List<iTimeTracker> listeners = new List<iTimeTracker>();

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
        //Initialise the time stamp
        timestamp = new gameTimestamp(0, gameTimestamp.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());

    }
    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1 / timeScale);
        }

    }
    //A tick of the in-game time
    public void Tick()
    {
        timestamp.UpdateClock();

        //Inform each of the listeners of the new time state 
        foreach (iTimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }

        UpdateSunMovement();
    }

    //Day and night cycle
    void UpdateSunMovement()
    {
        //Convert the current time to minutes
        int timeInMinutes = gameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;

        //Sun moves 15 degrees in an hour
        //.25 degrees in a minute
        //At midnight (0:00), the angle of the sun should be -90
        float sunAngle = .25f * timeInMinutes - 90;

        //Apply the angle to the directional light
        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    //Get the timestamp
    public gameTimestamp GetGameTimestamp()
    {
        //Return a cloned instance
        return new gameTimestamp(timestamp);
    }

    //Handling Listeners

    //Add the object to the list of listeners
    public void RegisterTracker(iTimeTracker listener)
    {
        listeners.Add(listener);
    }

    //Remove the object from the list of listeners
    public void UnregisterTracker(iTimeTracker listener)
    {
        listeners.Remove(listener);
    }

}
