using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CharacterController3D : MonoBehaviour
{
	[Header("Movement Modifiers")]
	[Range(0, 1)] 	[SerializeField] private float	m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float 	m_MovementSmoothing = .05f;		// How much to smooth out the movement
	public LayerMask m_WhatIsGround;												// A mask determining what is ground to the character
	
	[Header("Jumping")]
	[SerializeField] private float 	m_JumpForce = 400f;								// Amount of force added when the player jumps.
	[SerializeField] private bool 	m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private float	m_FallMultiplier = 2.5f;						// Adds extra Gravity to the Fall
	[SerializeField] private float 	m_LowJumpMultiplier = 2.0f;						// needed for Jumping higher by hold the Jump button
	
	[Header("Checkers")]
	[SerializeField] private Transform 	m_GroundCheck;								// A position marking where to check if the player is grounded.
	[SerializeField] private Transform 	m_CeilingCheck;								// A position marking where to check for ceilings
	[SerializeField] private Collider 	m_CrouchDisableCollider;					// A collider that will be disabled when crouching
	
	[Header("Effects")]
	[SerializeField] private ParticleSystem[] m_JumpParticleSpawner;					//ParticleSystems to the Feet of the Character
	private float PS_spawnToFeetOffsetY = -0.8f;
	private float PS_spawnToFeetOffsetX = 0.8f;

	public float 		k_CeilingRadius = 0.2f; 			// Radius of the overlap circle to determine if the player can stand up
	public float 		k_GroundedRadius = 0.2f; 			// Radius of the overlap circle to determine if grounded
	private bool 		m_isGrounded = false;            	// Whether or not the player is grounded.

	private Rigidbody 	m_Rigidbody;
	private bool		m_FacingRight = true;  				// For determining which way the player is currently facing.
	private Vector3		m_Velocity = Vector3.zero;

	// Shapeshifting
	private ShapeshiftController shapeshiftController;
	
	[Header("Events")][Space]
	public UnityEvent	OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent 	OnCrouchEvent;
	private bool		m_wasCrouching = false;

	// --------------------------------

	public float MyJumpforce
	{
		get {return m_JumpForce;}
		set {m_JumpForce = value;}
	}
	
	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		shapeshiftController = GetComponent<ShapeshiftController>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

	}

	private bool draw = true;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (draw)
		{
			//Gizmos.DrawWireCube(m_GroundCheck.position, m_GroundCheck.transform.localScale);
			Gizmos.DrawSphere(m_GroundCheck.position , k_GroundedRadius);
		}
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_isGrounded;
		m_isGrounded = false;

		//m_isGrounded = Physics.CheckSphere(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround, QueryTriggerInteraction.Ignore);

		Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround, QueryTriggerInteraction.Ignore);
		//Collider[] colliders = Physics.OverlapBox(m_GroundCheck.position, m_GroundCheck.localScale / 2, Quaternion.identity,m_WhatIsGround, QueryTriggerInteraction.Ignore);

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != this.gameObject)
			{
				m_isGrounded = true;

				if (!wasGrounded && m_Rigidbody.velocity.y < 0)
				{
					//Debug.Log("Onland" + m_Rigidbody.velocity.y);
					SpawnParticle(false);
					OnLandEvent.Invoke();
				}

			}
		}
		
		//If Falling, add bit velocity to Falling 
		if (m_Rigidbody.velocity.y > 0)
		{
			m_Rigidbody.velocity += Vector3.up * Physics2D.gravity.y * (m_FallMultiplier -1) * Time.fixedDeltaTime;
		}
		// Hold Jumpbutton longer jump higher
		else if (m_Rigidbody.velocity.y > 0 && !Input.GetButton("Jump")) 
		{
			m_Rigidbody.velocity += Vector3.up * Physics2D.gravity.y * (m_LowJumpMultiplier -1) * Time.fixedDeltaTime;
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics.OverlapSphere(m_CeilingCheck.position,k_CeilingRadius,m_WhatIsGround).Length > 0)
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_isGrounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				Flip();
			}
		}

		// If the player should jump Add a vertical force to the player.
		if (jump && m_isGrounded)
		{
			m_isGrounded = false;
			m_Rigidbody.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);
			SpawnParticle(true);
		}
	}
	
	/// <summary>
	/// Spawn Particle to the Feet depending if Jumping or landing
	/// </summary>
	/// <param name="jump"></param>
	private void SpawnParticle(bool jump)
	{
		if (jump)
		{
			PS_spawnToFeetOffsetX *= -1;
		}

		for (int j = 0; j < m_JumpParticleSpawner.Length; j++)
		{
			m_JumpParticleSpawner[j].transform.position = this.transform.position + (new Vector3(PS_spawnToFeetOffsetX, PS_spawnToFeetOffsetY, 0.0f));
			m_JumpParticleSpawner[j].Play();
		}
	}

	/// <summary>
	/// // Switch the way the player is labelled as facing.
	/// </summary>
	private void Flip()
	{
		m_FacingRight = !m_FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
