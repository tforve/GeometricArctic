using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;
	
	public float runSpeed = 40.0f;
		
	private float horizontalMove = 0.0f;

	private bool jump = false;
	private bool crouch = false;
	
	

	// Update is called once per frame
	void Update ()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
		
		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		}
	}

	private void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
		
	}
}
