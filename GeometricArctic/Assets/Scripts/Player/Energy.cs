using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Energy : MonoBehaviour
{
    private static Energy instance;

    public static Energy MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Energy>();
            }
            return instance;
        }
    }

    // ------------

    [Header("Energy")]
    [SerializeField] private int energy;
    [SerializeField] private int maxEnergy;
    [SerializeField] private Image[] energies;
    [SerializeField] private Sprite emptyEnergy_even, emptyEnergy_odd;
    [SerializeField] private Sprite filledEnery_even, filledEnery_odd;
    private bool isTimerRunning = false;                                    //boolean for Coroutine 

    [Header("Health")]
    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 1;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI noEnergy;

    private GameMaster gameMaster;

    //Get & set Energy from other scripts
    public int MyEnergy
    {
        get { return energy; }
        set { energy = value; }
    }

    public int MyHealth
    {
        get { return health; }
        set { health = value; }
    }

    void Start()
    {
        UpdateEnergy();
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    // ---------- ENERGY -------------

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

    /// <summary> Enery to shapeshift is empty </summary> 
    public void Empty()
    {
        if (!isTimerRunning)
        {
            StartCoroutine(textEnableTimer(4.0f));
        }
        energy = 0;
    }

    private IEnumerator textEnableTimer(float waitTime)
    {
        isTimerRunning = true;
        noEnergy.enabled = true;
        yield return new WaitForSeconds(waitTime);
        isTimerRunning = false;
        noEnergy.enabled = false;
    }

    /// <summary> Update Current Energy to correct amount.</summary>
    private void UpdateEnergy()
    {
        if (energy > maxEnergy) energy = maxEnergy;

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

    // ---------- HEALTH -------------

    /// <summary>
    /// Health of player.
    /// </summary>
    /// <param name="dmg">the amount of Damage done to Player</param>
    public void Hit(int dmg)
    {
        health -= dmg;
        UpdateHealth();
    }

    ///<summary> Replanish amount of health </summary>
    public void ReplanishHealth(int hp)
    {
        health += hp;
        UpdateHealth();
    }

    /// <summary>
    /// Player Dies. Needs to be implemented.
    /// </summary>> 
    private void Die()
    {
        // cooler Restart scene needed
        gameMaster.LoadCurrentScene();
    }

    private void UpdateHealth()
    {
        if (health >= maxHealth) health = maxHealth;
        if (health <= 0) Die();
    }


}
