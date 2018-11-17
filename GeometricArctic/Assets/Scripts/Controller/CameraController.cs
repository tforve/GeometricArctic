using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField] private GameObject target;
	[SerializeField] private float offsetY = 0.25f, offsetZ = 3.0f;
	

	void Update()
	{
		if (target)
		{
			transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.1f) + new Vector3(0, offsetY, -offsetZ);
		}
	}

}
