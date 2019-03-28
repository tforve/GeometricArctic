using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{

    private GameMaster gameMaster;
    [SerializeField] private GameObject text;
    [SerializeField] private PlayerMovement playerMovement;
    private bool isOnTrigger = false;                           // bool to check if player is near to Collider

    // --------------

    public Vector3 MyCheckpointPos
    {
        get { return this.transform.position; }
    }

    public bool MyIsOnTrigger
    {
        get { return isOnTrigger;}
    }
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
            isOnTrigger = true;
            playerMovement.MyLastCheckpointPos = this.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false);
            isOnTrigger = false;
        }
    }
}
