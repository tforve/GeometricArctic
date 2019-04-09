using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trigger : MonoBehaviour
{
    private ShapeshiftController shapeshiftController;
    private PlayerMovement playerMovement;
    private Interactables interactableTyp;                         // set Type of Interactable

    // Trigger
    private bool isUsed = false;
    [SerializeField] private Light triggerLight;                // TriggerLight to change Color on Use Trigger

    // Door
    [SerializeField] GameObject door;                           // the door that should be opened
    private Animator doorAnim;                                  // Animation of the Door referenced
    [SerializeField] private float delayTime;                   // time till door opening starts
    private Vector3 endPos, startPos;                           // positions Door starts and ends;

    // GuiText
    [SerializeField] private GameObject txt_useTrigger;         // Text showing up when player is in Range of Trigger
    [SerializeField] private GameObject txt_WrongForm;          // If Player has the wrong Form

    void Start()
    {
        shapeshiftController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShapeshiftController>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        doorAnim = door.GetComponent<Animator>();

        interactableTyp = Interactables.trigger;

        startPos = this.transform.position;
        endPos = this.transform.position;
        endPos += new Vector3(0.0f, 10.0f, 0.0f);
        txt_useTrigger.SetActive(false);
        txt_WrongForm.SetActive(false);

    }

    public void TriggerHandle()
    {
        if (shapeshiftController.MyCurrenShape != Shapes.human)
        {
            txt_WrongForm.SetActive(true);
            txt_useTrigger.SetActive(false);
        }
        else if (!isUsed)
        {
            triggerLight.color = Color.green;
            isUsed = true;

            // what should the Trigger Do
           doorAnim.SetBool("isOpening", true);
           
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            txt_useTrigger.SetActive(true);
            playerMovement.MyIsOnTrigger = true;
            playerMovement.MyCurrentTrigger = this;
            playerMovement.MyCurrentInteractable = interactableTyp;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            txt_WrongForm.SetActive(false);
            txt_useTrigger.SetActive(false);
            playerMovement.MyIsOnTrigger = false;
        }
    }

}
