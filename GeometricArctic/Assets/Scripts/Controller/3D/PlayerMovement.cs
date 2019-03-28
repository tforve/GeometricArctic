using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

// Forms the Player can switch to 
public enum Shapes { human, fox, bear, seal };

[RequireComponent(typeof(CharacterController3D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float runSpeed = 40.0f;
    [SerializeField] private Animator animator;
    private CharacterController3D controller;
    private float horizontalMove = 0.0f;
    private bool jump = false;
    private bool crouch = false;

    // Shapeshifting
    private ShapeshiftController shapeshiftController;
    // Interact
   // private Checkpoint lastCheckpoint;         //last saved Checkpoint, at the start of the game == Levelstart
    private Vector3 lastCheckpointPos;           // transform.position of last Checkpoint collided with
    private GameMaster gameMaster;
    private bool isOnTrigger = true;                    // bool to check if near to interactable Trigger


    // ---------------------

    public float MyRunSpeed
    {
        get { return runSpeed; }
        set { runSpeed = value; }
    }

    public Vector3 MyLastCheckpointPos
    {
        set {lastCheckpointPos = value;}
    }

    public bool MyIsOnTrigger
    {
        set {isOnTrigger = value;}
    }

    // --------------------

    void Start()
    {
        controller = GetComponent<CharacterController3D>();
        shapeshiftController = GetComponent<ShapeshiftController>();
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
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
            crouch = true;
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
        //Interacte
        if (Input.GetButtonDown("Interact") && isOnTrigger == true)
        {
            //need to check what type of Interactable it is
            //get checkPoint colliding with
            gameMaster.SetLastCheckpoint(lastCheckpointPos);
        }

        //Delete later
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
