using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField] private GameObject target;

	void Update()
	{
		if (target)
		{
			transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.1f) + new Vector3(0, 1.0f ,-3.8f);
		}
	}

}
