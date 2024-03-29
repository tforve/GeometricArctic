﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{

   // private GameMaster gameMaster;
    private PlayerMovement playerMovement;
    private Interactables interactable;

    [SerializeField] private GameObject text;                       // text displayed when player is standing near to Checkpoint

    // --------------

    void Start()
    {
       //gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        text.SetActive(false);
        interactable = Interactables.checkpoint;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(true);
            playerMovement.MyIsOnTrigger = true;                     // has to be done with all interactables
            playerMovement.MyLastCheckpointPos = this.transform.position;
            playerMovement.MyCurrentInteractable = interactable;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false);
            playerMovement.MyIsOnTrigger = false;
        }
    }
}
