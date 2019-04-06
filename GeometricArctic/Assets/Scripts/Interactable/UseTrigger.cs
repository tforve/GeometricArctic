using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UseTrigger : MonoBehaviour
{
    private Light halo;
    private bool isUsed = false;
    private float timeToReachTarget;
    private Vector3 target, startPos;
    float lerpTime;

    void Start()
    {
        halo = GetComponent<Light>();
        startPos = this.transform.position;
        target = startPos.transform.Translate(0.0f, -0.25f, 0.0f);
    }

    private void FlickeringHalo()
    {
        halo.color = Color.green;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            lerpTime += Time.deltaTime / timeToReachTarget;
            this.transform.position = Vector3.Lerp(startPos, target, lerpTime);//Translate(0.0f, -0.25f, 0.0f);
            isUsed = true;
        }
    }

    public void SetDestination(Vector3 destination, float time)
    {
        lerpTime = 0;
        startPos = transform.position;
        timeToReachTarget = time;
        target = destination;
    }

}
