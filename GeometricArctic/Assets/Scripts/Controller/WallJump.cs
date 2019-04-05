using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    [SerializeField] private float maxDistance = 2.85f;
    private CharacterController3D characterController;
    private ShapeshiftController shapeshiftController;
    private PlayerMovement playerMovement;
    public LayerMask m_WhatIsWall;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController3D>();
        shapeshiftController = GetComponent<ShapeshiftController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (shapeshiftController.MyCanWalljump)
        {
            if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.right), out hit, maxDistance, m_WhatIsWall) || Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.left), out hit, maxDistance, m_WhatIsWall))
            {
                playerMovement.TouchWall();
                characterController.MyIsGrounded = true;
                // add new Animation on Wall

                // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * maxDistance, Color.red);
            }
            else
            {
                
            }
        }
    }
}
