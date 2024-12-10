using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public float offsetZ = 5f;
    public float offsety = 5f;
    public float smoothing = 2f;

    Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        playerPos =  FindAnyObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer();
    }

    public void followPlayer()
    {
        Vector3 target = new Vector3(playerPos.position.x, playerPos.position.y + offsety, playerPos.position.z - offsetZ);
        transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
    }
}
