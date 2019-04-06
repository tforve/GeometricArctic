using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    //Range for min/max values of variable
    [Range(-10f, 10f)]
    public float cloudsMoveSpeedX, cloudsMoveSpeedZ;

    // Clouds Movement
    void Update()
    {
        gameObject.transform.Translate(cloudsMoveSpeedX * Time.deltaTime, 0f, cloudsMoveSpeedZ * Time.deltaTime);
    }
}
