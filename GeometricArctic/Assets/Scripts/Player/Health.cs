using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private static Health instance;

    public static Health MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Health>();
            }
            return instance;
        }
    }

    // ------------

    [SerializeField] private int energy;
    [SerializeField] private int maxEnergy;
    [SerializeField] private Image[] energies;
    [SerializeField] private Sprite emptyEnergy_even, emptyEnergy_odd;
    [SerializeField] private Sprite filledEnery_even, filledEnery_odd;

    private GameMaster gameMaster;

    //Get & set Health from other scripts
    public int MyEnergy
    {
        get { return energy; }
        set { energy = value; }
    }

    void Start()
    {
        UpdateEnergy();
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    /// <summary>
    /// Use Energy of player.
    /// </summary>
    /// <param name="eng">the amount of Energy needed</param>
    public void DrainEnergy(int eng)
    {
        energy -= eng;
        UpdateEnergy();
    }

    ///<summary> Replanish amount of energy </summary>
    public void ReplanishEnergy(int eng)
    {
        energy += eng;
        UpdateEnergy();
    }

    /// <summary>
    /// Player Dies. Needs to be implemented.
    /// </summary>> 
    private void Empty()
    {
        Debug.Log("Player is Dead");
        // Reset to last Savepoint

        // Debug Only. Delete later
        //gameMaster.LoadCurrentScene();

    }

    /// <summary>
    /// Update Current Energy to correct amount.
    /// </summary>
    private void UpdateEnergy()
    {
        if (energy > maxEnergy) energy = maxEnergy;
        if (energy <= 0) Empty();


        for (int currentTriangle = 0; currentTriangle < energies.Length; currentTriangle++)
        {
            //set correct amount of Triangles
            if (currentTriangle < maxEnergy)
            {
                energies[currentTriangle].enabled = true;
            }
            else
            {
                energies[currentTriangle].enabled = false;
            }

            // check if lower health then maxHealth
            if (currentTriangle < energy)
            {
                int tmp = currentTriangle & 1;
                if (tmp == 0)
                {
                    energies[currentTriangle].sprite = filledEnery_even;
                }
                else
                {
                    energies[currentTriangle].sprite = filledEnery_odd;
                }
            }
            else
            {
                int tmp = currentTriangle & 1;
                if (tmp == 0)
                {
                    energies[currentTriangle].sprite = emptyEnergy_even;
                }
                else
                {
                    energies[currentTriangle].sprite = emptyEnergy_odd;
                }


            }
        }
    }


}
