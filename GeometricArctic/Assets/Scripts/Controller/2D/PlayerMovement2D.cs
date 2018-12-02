using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PlayerMovement2D : MonoBehaviour
{

	public CharacterController2D controller;
	public float runSpeed = 40.0f;

	[SerializeField] private Animator animator;
	
	private float horizontalMove = 0.0f;

	private bool jump = false;
	private bool crouch = false;
	
	

	// Update is called once per frame
	void Update ()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		
		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("HasLanded", false);
			animator.SetBool("IsJumping", true);
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		}
		else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}
	}

	public void OnCrouching(bool isCrouching)
	{
		animator.SetBool("IsCrouching", isCrouching);
	}

	public void OnLanding()
	{
		Debug.Log("OnLanding() triggered");
		//triggered twice. also when jumping
		animator.SetBool("HasLanded", true);
		animator.SetBool("IsJumping", false);
	}

	private void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
	

}
