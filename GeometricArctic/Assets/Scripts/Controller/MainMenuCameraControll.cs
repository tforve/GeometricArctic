using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraControll : MonoBehaviour
{
    //Range for min/max values of variable
    [Range(-100f, 100f)]
    public float cameraSpeedX, cameraSpeedY, cameraSpeedZ;

    // Camera Movement
    void Update()
    {
        gameObject.transform.Translate(cameraSpeedX * Time.deltaTime, cameraSpeedY * Time.deltaTime, cameraSpeedZ * Time.deltaTime);
    }
}
