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
    
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private Image[] healths;
    [SerializeField] private Sprite emptyHealth_even, emptyHealth_odd;
    [SerializeField] private Sprite filledHealth_even, filledHealth_odd;

   
    //Get & set Health from other scripts
    public int MyHealth
    {
        get { return health; }
        set { health = value; }
    }

    void Start()
    {
        UpdateHealth();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Hit(2);
        }
    }

    /// <summary>
    /// Hit player with amount of Damage.
    /// </summary>
    /// <param name="dmg"></param>
    public void Hit(int dmg)
    {
        health -= dmg;
        UpdateHealth();
    }
    
    /// <summary>
    /// Player Dies. Needs to be implemented.
    /// </summary>> 
    private void Die()
    {
        Debug.Log("Player is Dead");
        // Reset to last Savepoint
    }
    
    /// <summary>
    /// Update Current Health to correct amount.
    /// </summary>
    private void UpdateHealth()
    {
        if (health > maxHealth) health = maxHealth;
        if (health <= 0) Die();
    

        for (int currentTriangle = 0; currentTriangle < healths.Length; currentTriangle++)
        {
            //set correct amount of Triangles
            if (currentTriangle < maxHealth)
            {
                    healths[currentTriangle].enabled = true;
            }
            else
            {
                healths[currentTriangle].enabled = false;
            }

            // check if lower health then maxHealth
            if (currentTriangle < health)
            {
                int tmp = currentTriangle & 1;
                if (tmp == 0)
                {
                    healths[currentTriangle].sprite = filledHealth_even;
                }
                else
                {
                    healths[currentTriangle].sprite = filledHealth_odd;
                }
            }
            else
            {
                int tmp = currentTriangle & 1;
                if (tmp == 0)
                {
                    healths[currentTriangle].sprite = emptyHealth_even;
                }
                else
                {
                    healths[currentTriangle].sprite = emptyHealth_odd;
                }
     

            }
        }
    }


}
