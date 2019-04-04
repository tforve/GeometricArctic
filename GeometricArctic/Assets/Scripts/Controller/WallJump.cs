using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    [SerializeField] private float maxDistance = 2.85f;
    private CharacterController3D characterController;
    public LayerMask m_WhatIsWall;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController3D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.right), out hit, maxDistance, m_WhatIsWall) ||Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.left), out hit, maxDistance, m_WhatIsWall) )
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * maxDistance, Color.red);
            characterController.MyIsGrounded = true;            
        }        
    }
}
