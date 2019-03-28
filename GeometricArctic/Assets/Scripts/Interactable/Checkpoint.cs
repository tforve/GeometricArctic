using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{

    private GameMaster gameMaster;
    [SerializeField] private GameObject text;
    [SerializeField] private PlayerMovement playerMovement;

    // --------------

    // --------------

    void Start()
    {
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        text.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(true);
            playerMovement.MyIsOnTrigger = true;            // has to be done with all interactables
            playerMovement.MyLastCheckpointPos = this.transform.position;
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
