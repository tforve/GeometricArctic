using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunControll : MonoBehaviour
{
    	//Range for min/max values of variable
	[Range(-10f, 10f)]
	public float XRotationSpeed, YRotationSpeed;

	// Sun Movement
	void Update () {
		gameObject.transform.Rotate (XRotationSpeed * Time.deltaTime, YRotationSpeed * Time.deltaTime, 0);
	}
}
