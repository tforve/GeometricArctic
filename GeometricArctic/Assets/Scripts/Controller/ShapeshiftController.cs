using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeshiftController : MonoBehaviour {

	//What have to change
	private Animator animator;												// have to change the Animator to the correct one for every Shape
	private CharacterController3D controller3D;								// used to change CharacterControll related values
	private PlayerMovement playerMovement;									// relevant for Inputs and to change runSpeed
	private Health 	resource;
	
	//Particle system for Shapeshifteffect
	[SerializeField] private ParticleSystem shapeShiftParticleSystem;		// Particle System triggered when ShapeShift is activated

	[Header("Human")]
	[SerializeField] private RuntimeAnimatorController 	h_Animation;
	[SerializeField] private BoxCollider 				h_BoxCollider;
	[SerializeField] private CapsuleCollider 			h_CapsulCollider;
	[Header("Fox")]
	[SerializeField] private RuntimeAnimatorController 	f_Animation;
	[SerializeField] private BoxCollider 				f_BoxCollider;
	[SerializeField] private CapsuleCollider 			f_CapsulCollider;
	[Header("Owl")]
	[SerializeField] private RuntimeAnimatorController 	o_Animation;
	[SerializeField] private BoxCollider 				o_BoxCollider;
	[SerializeField] private CapsuleCollider 			o_CapsulCollider;
	[Header("Seal")]
	[SerializeField] private RuntimeAnimatorController 	s_Animation;
	[SerializeField] private BoxCollider				s_BoxCollider;
	[SerializeField] private CapsuleCollider 			s_CapsulCollider;

	private List<Collider> oldColliders = new List<Collider>();				// List of all active Colliders
	
	// --------------------------------------------
	
	void Awake()
	{
		HandleColliders();
		h_BoxCollider.enabled = true;
		h_CapsulCollider.enabled = true;
	}

	void Start()
	{
		animator = GetComponent<Animator>();
		controller3D = GetComponent<CharacterController3D>();
		playerMovement = GetComponent<PlayerMovement>();
		resource = GetComponent<Health>();
	}

	/// <summary>
	/// Switch active Colliders. Disable wrong Colliders, enables correct ones
	/// </summary>
	void HandleColliders()
	{
		oldColliders.Clear();

		foreach (Collider collider in GetComponentsInChildren<Collider>())
		{
			oldColliders.Add(collider);
		}
		
		foreach (var collider in oldColliders)
		{
			collider.enabled = false;
		}
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
				h_BoxCollider.enabled = true;
				h_CapsulCollider.enabled = true;
				// animator austauschen
				animator.runtimeAnimatorController = h_Animation;
				// evtl. controlls austauschen

				// movespeed, jumpForce etc. anpassen
				controller3D.MyJumpforce = 15;
				playerMovement.MyRunSpeed = 7;
				// ressource Cost for Shapeshift
				resource.Hit(1);
				break;

			case Shapes.fox:
				f_BoxCollider.enabled = true;
				f_CapsulCollider.enabled = true;
				animator.runtimeAnimatorController = f_Animation;
				controller3D.MyJumpforce = 15;
				playerMovement.MyRunSpeed = 10;
				resource.Hit(1);
				break;

			case Shapes.owl:
				o_BoxCollider.enabled = true;
				o_CapsulCollider.enabled = true;
				animator.runtimeAnimatorController = o_Animation;
				controller3D.MyJumpforce = 0;
				playerMovement.MyRunSpeed = 8;
				resource.Hit(1);
				break;

			case Shapes.seal:
				s_BoxCollider.enabled = true;
				s_CapsulCollider.enabled = true;
				animator.runtimeAnimatorController = s_Animation;
				controller3D.MyJumpforce = 3;
				playerMovement.MyRunSpeed = 5;
				resource.Hit(1);
				break;
		}
		
	}


}
