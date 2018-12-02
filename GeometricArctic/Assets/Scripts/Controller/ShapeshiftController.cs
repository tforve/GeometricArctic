using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeshiftController : MonoBehaviour {

	//What have to change
	private Animator animator;										// have to change the Animator to the correct one for every Shape
	private SpriteRenderer spriteRenderer;

	private CharacterController3D controller3D;						// used to change CharacterControll related values
	private PlayerMovement playerMovement;							// relevant for Inputs and to change runSpeed
	private Health resource;

	[Header("Human")]
	[SerializeField] private Animator h_Animator;
	[SerializeField] private BoxCollider h_BoxCollider;
	[SerializeField] private CapsuleCollider h_CapsulCollider;
	[Header("Fox")]
	[SerializeField] private Animator f_Animator;
	[SerializeField] private BoxCollider f_BoxCollider;
	[SerializeField] private CapsuleCollider f_CapsulCollider;

	private List<Collider> oldColliders;							// List of all active Colliders

	void Start()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		controller3D = GetComponent<CharacterController3D>();
		playerMovement = GetComponent<PlayerMovement>();
		resource = GetComponent<Health>();
	}

	///<summary> Update and Handles alle Colliders </summary>
	void HandleColliders()
	{
		//alle Collider durchlaufen und die jetzt AKTIVEN.enable = false setzen.
		//oldColliders.Add(GetComponent<Collider>());
		//Debug.Log(oldColliders.Count);
	}
	
	/// <summary>
	/// Handle the complete Shapeshifting. 
	/// </summary>
	/// <param name="shapes"></param>
	public void SwitchShape(Shapes shapes)
	{
		HandleColliders();

		switch (shapes)
		{
			case Shapes.human:
				// collider aktivieren

				// animator austauschen

				// evtl. controlls austauschen

				// movespeed, jumpForce etc. anpassen
				controller3D.MyJumpforce = 15;
				playerMovement.MyRunSpeed = 7;
				resource.Hit(1);
				break;

			case Shapes.fox:
				// collider aktivieren

				// animator austauschen

				// evtl. controlls austauschen

				// movespeed, jumpForce etc. anpassen
				controller3D.MyJumpforce = 15;
				playerMovement.MyRunSpeed = 10;
				resource.Hit(1);
				break;

			case Shapes.owl:
				// collider aktivieren

				// animator austauschen

				// evtl. controlls austauschen

				// movespeed, jumpForce etc. anpassen
				controller3D.MyJumpforce = 0;
				playerMovement.MyRunSpeed = 8;
				break;

			case Shapes.robbe:
				// collider aktivieren

				// animator austauschen

				// evtl. controlls austauschen

				// movespeed, jumpForce etc. anpassen
				controller3D.MyJumpforce = 3;
				playerMovement.MyRunSpeed = 5;
				break;
		}
		
	}


}
