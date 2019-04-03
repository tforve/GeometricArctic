using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShapeshiftController : MonoBehaviour
{

    //What have to change
    private Animator animator;                                              // have to change the Animator to the correct one for every Shape
    private CharacterController3D controller3D;                             // used to change CharacterControll related values
    private PlayerMovement playerMovement;                                  // relevant for Inputs and to change runSpeed
    private Health resource;
    private Shapes currentShape = Shapes.human;                               // current shape, needed for checks

    //Particle system for Shapeshifteffect
    [SerializeField] private ParticleSystem shapeShiftParticleSystem;
    private ParticleSystem[] shapeShiftPsList;// Particle System triggered when ShapeShift is activated
    [SerializeField] private float shapeShiftTime = 0.5f;

    //Crouching
    private bool canCrouch = false;
    public bool MyCanCrouch
    {
        get { return canCrouch; }
    }

    [Header("Girl")]
    [SerializeField] private RuntimeAnimatorController g_Animation;
    [SerializeField] private BoxCollider g_BoxCollider;
    [SerializeField] private CapsuleCollider g_CapsulCollider;
    [SerializeField] private float g_runSpeed = 7.0f;
    [SerializeField] private float g_jumpForce = 15.0f;
    [SerializeField] private bool g_canCrouch = false;
    [Header("Fox")]
    [SerializeField] private RuntimeAnimatorController f_Animation;
    [SerializeField] private BoxCollider f_BoxCollider;
    [SerializeField] private CapsuleCollider f_CapsulCollider;
    [SerializeField] private float f_runSpeed = 10.0f;
    [SerializeField] private float f_jumpForce = 15.0f;
    [SerializeField] private bool f_canCrouch = true;
    [Header("Bear")]
    [SerializeField] private RuntimeAnimatorController b_Animation;
    [SerializeField] private BoxCollider b_BoxCollider;
    [SerializeField] private CapsuleCollider b_CapsulCollider;
    [SerializeField] private float b_runSpeed = 8.0f;
    [SerializeField] private float b_jumpForce = 2.0f;
    [SerializeField] private bool b_canCrouch = false;
    [Header("Seal")]
    [SerializeField] private RuntimeAnimatorController s_Animation;
    [SerializeField] private BoxCollider s_BoxCollider;
    [SerializeField] private CapsuleCollider s_CapsulCollider;
    [SerializeField] private float s_runSpeed = 5.0f;
    [SerializeField] private float s_jumpForce = 3.0f;
    [SerializeField] private bool s_canCrouch = false;

    private List<Collider> oldColliders = new List<Collider>();             // List of all active Colliders
    private SpriteRenderer spriteRenderer;

    // --------------------------------------------

    void Awake()
    {
        HandleColliders();
        g_BoxCollider.enabled = true;
        g_CapsulCollider.enabled = true;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        controller3D = GetComponent<CharacterController3D>();
        playerMovement = GetComponent<PlayerMovement>();
        resource = GetComponent<Health>();

        //particle ShapeShift stuff
        shapeShiftPsList = shapeShiftParticleSystem.GetComponentsInChildren<ParticleSystem>();

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

    void StartShapeshiftEffect()
    {
        //change Shape to current Playerform
        if (shapeShiftPsList.Length != null)
        {
            for (int i = 0; i < shapeShiftPsList.Length; i++)
            {
                var shape = shapeShiftPsList[i].shape;
                shape.texture = this.spriteRenderer.sprite.texture;
            }

        }

        // change position to actual Player.transform.position
        shapeShiftParticleSystem.transform.position = this.transform.position;
        //flip direction same as Player

        // Play() effect
        shapeShiftParticleSystem.Play();

        //deactive sprite of player
        StartCoroutine(ShapeShiftTime(shapeShiftTime));

        // start External Force
        //at the moment its the Unity Forcefield component

    }

    private IEnumerator ShapeShiftTime(float shapeShiftTime)
    {
        Color tmp = spriteRenderer.color;
        tmp.a = 0.0f;
        spriteRenderer.color = tmp;

        yield return new WaitForSeconds(shapeShiftTime);

        Color tmp2 = spriteRenderer.color;
        tmp2.a = 255.0f;
        spriteRenderer.color = tmp2;
    }

    /// <summary>
    /// Handle the complete Shapeshifting. Enum of possible shapes is located in PlayerMovement.cs
    /// </summary>
    /// <param name="shapes"> human, fox, bear, seal</param>
    public void SwitchShape(Shapes shapes)
    {
        if (currentShape != shapes)
        {
            currentShape = shapes;

            HandleColliders();
            //start effect
            StartShapeshiftEffect();

            //change ShapeshiftForm to actual Form		
            switch (shapes)
            {
                case Shapes.human:
                    // collider aktivieren
                    g_BoxCollider.enabled = true;
                    g_CapsulCollider.enabled = true;
                    // animator austauschen
                    animator.runtimeAnimatorController = g_Animation;
                    // evtl. controlls austauschen
                    // movespeed, jumpForce etc. anpassen
                    playerMovement.MyRunSpeed = g_runSpeed;
                    controller3D.MyJumpforce = g_jumpForce;
                    // set crouch
                    canCrouch = g_canCrouch;
                    // ressource Cost for Shapeshift
                    resource.Hit(1);
                    break;

                case Shapes.fox:
                    f_BoxCollider.enabled = true;
                    f_CapsulCollider.enabled = true;
                    // special Crouch collider
                    controller3D.SetCrouchCollider();
                    animator.runtimeAnimatorController = f_Animation;
                    playerMovement.MyRunSpeed = f_runSpeed;
                    controller3D.MyJumpforce = f_jumpForce;
                    canCrouch = f_canCrouch;
                    resource.Hit(1);
                    break;

                case Shapes.bear:
                    b_BoxCollider.enabled = true;
                    b_CapsulCollider.enabled = true;
                    animator.runtimeAnimatorController = b_Animation;
                    playerMovement.MyRunSpeed = b_runSpeed;
                    controller3D.MyJumpforce = b_jumpForce;
                    canCrouch = b_canCrouch;
                    resource.Hit(1);
                    break;

                case Shapes.seal:
                    s_BoxCollider.enabled = true;
                    s_CapsulCollider.enabled = true;
                    animator.runtimeAnimatorController = s_Animation;
                    playerMovement.MyRunSpeed = s_runSpeed;
                    controller3D.MyJumpforce = s_jumpForce;
                    canCrouch = s_canCrouch;
                    resource.Hit(1);
                    break;
            }

        }
    }


}
