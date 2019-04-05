using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public enum Shapes { human, fox, bear, seal };                          // Forms the Player can switch to 
public enum Interactables { checkpoint, block, handle };                // Interactables Player can use

[RequireComponent(typeof(CharacterController3D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float runSpeed;
    [SerializeField] private Animator animator;
    private CharacterController3D controller;
    private float horizontalMove = 0.0f;
    private bool jump = false;
    private bool crouch = false;

    // Shapeshifting
    private ShapeshiftController shapeshiftController;

    // Interact
    private GameMaster gameMaster;
    private Interactables interactables;
    private Vector3 lastCheckpointPos;                                  // transform.position of last Checkpoint collided with
    private bool isOnTrigger = false;                                   // bool to check if near to interactable Trigger, has to be changed on ALL Interactables

    // Energy for debugging
    private Energy energy;

    // ---------------------

    public float MyRunSpeed
    {
        get { return runSpeed; }
        set { runSpeed = value; }
    }

    public Vector3 MyLastCheckpointPos
    {
        set { lastCheckpointPos = value; }
    }

    public bool MyIsOnTrigger
    {
        set { isOnTrigger = value; }
    }

    // --------------------

    void Start()
    {
        controller = GetComponent<CharacterController3D>();
        shapeshiftController = GetComponent<ShapeshiftController>();
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        energy = GetComponent<Energy>();
    }

    // Complete Input
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * 10.0f; //Why so slow

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("HasLanded", false);
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            if (shapeshiftController.MyCanCrouch)           // only fox can Crouch
            {
                crouch = true;
            }
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
        //Shapeshifting
        if (Input.GetButtonDown("ShiftToHuman"))
        {
            shapeshiftController.SwitchShape(Shapes.human);
        }
        if (Input.GetButtonDown("ShiftToFox"))
        {
            shapeshiftController.SwitchShape(Shapes.fox);
        }
        if (Input.GetButtonDown("ShiftToBear"))
        {
            shapeshiftController.SwitchShape(Shapes.bear);
        }
        if (Input.GetButtonDown("ShiftToSeal"))
        {
            shapeshiftController.SwitchShape(Shapes.seal);
        }
        //Interact
        if (Input.GetButtonDown("Interact") && isOnTrigger == true)
        {
            //need to check what type of Interactable it is
            switch (interactables)
            {
                case Interactables.checkpoint:
                    gameMaster.SetLastCheckpoint(lastCheckpointPos);
                    break;
                case Interactables.block:
                    // need bearForm - can Push and pull heavy Objects
                    break;
                case Interactables.handle:
                    // need girlForm - can use handles
                    break;
            }
        }

        //Delete later
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            energy.ReplanishEnergy(1);
        }

    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    public void OnLanding()
    {
        animator.SetBool("HasLanded", true);
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }


}
