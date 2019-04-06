using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UseTrigger : MonoBehaviour
{
    private Light halo;

    void Start()
    {
        halo = GetComponent<Light>();
    }

    private void FlickeringHalo()
    {
        halo.color = Color.green;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // FlickeringHalo();
        }
    }

}
