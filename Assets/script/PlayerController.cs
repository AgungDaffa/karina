using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Animator animator;

    private float moveSpeed = 4f;

    [Header("Movement System")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;


    PlayerInteraction playerInteraction;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInteraction = GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        move();

        Interact();

        //Debugging purposes only
        //Skip the time when the right square bracket is pressed
        if (Input.GetKey(KeyCode.RightBracket))
        {
            timeManager.Instance.Tick();
        }
    }

    public void Interact()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            playerInteraction.Interact();
        }

        //Item interaction
        if (Input.GetButtonDown("Fire2"))
        {
            playerInteraction.ItemInteract();
        }

    }

    public void move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //direction
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * dir;

        //sprint checker
        if (Input.GetButton("Sprint"))
        {
            moveSpeed = runSpeed;
            animator.SetBool("running", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("running", false);
        }

        //check movement
        if(dir.magnitude > 0.1f)
        {
            //look toward that direction
            transform.rotation = Quaternion.LookRotation(dir);

            //move
            controller.Move(velocity);
        }

        animator.SetFloat("speed", velocity.magnitude);
    }
}
