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
    private Shapes currentShape = Shapes.fox;                             // current shape, needed for checks

    //Particle system for Shapeshifteffect
    [SerializeField] private ParticleSystem shapeShiftParticleSystem;
    private ParticleSystem[] shapeShiftPsList;// Particle System triggered when ShapeShift is activated
    [SerializeField] private float shapeShiftTime = 0.5f;

    [Header("Girl")]
    [SerializeField] private RuntimeAnimatorController g_Animation;
    [SerializeField] private BoxCollider g_BoxCollider;
    [SerializeField] private CapsuleCollider g_CapsulCollider;
    [Header("Fox")]
    [SerializeField] private RuntimeAnimatorController f_Animation;
    [SerializeField] private BoxCollider f_BoxCollider;
    [SerializeField] private CapsuleCollider f_CapsulCollider;
    [Header("Bear")]
    [SerializeField] private RuntimeAnimatorController b_Animation;
    [SerializeField] private BoxCollider b_BoxCollider;
    [SerializeField] private CapsuleCollider b_CapsulCollider;
    [Header("Seal")]
    [SerializeField] private RuntimeAnimatorController s_Animation;
    [SerializeField] private BoxCollider s_BoxCollider;
    [SerializeField] private CapsuleCollider s_CapsulCollider;

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
    /// Handle the complete Shapeshifting. 
    /// </summary>
    /// <param name="shapes"></param>
    public void SwitchShape(Shapes shapes)
    {
        if (currentShape != shapes)
        {
            currentShape = shapes;

            HandleColliders();
            //change ShapeshiftForm to actual Form		

            //need to Check if it isnt already in that form. Only if NOT start shapeshift

            //start effect
            StartShapeshiftEffect();

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

                case Shapes.bear:
                    b_BoxCollider.enabled = true;
                    b_CapsulCollider.enabled = true;
                    animator.runtimeAnimatorController = b_Animation;
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


}
