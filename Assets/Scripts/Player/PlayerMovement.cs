using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 moveDirection;
    public float speed = 5f;
    private float gravity = 20f;
    public float jumpForce = 10f;
    private float verticalVelocity;
    
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        moveDirection = new Vector3(
                                        Input.GetAxis("Horizontal"), 
                                        0f, 
                                        Input.GetAxis("Vertical")
                                    );

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;
        
        ApplyGravity();
        characterController.Move(moveDirection);
    }

    private void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if(characterController.isGrounded)
        {
            verticalVelocity = -1 * Time.deltaTime;
        }
        PlayerJump();
        moveDirection.y = verticalVelocity * Time.deltaTime;
    }

    private void PlayerJump()
    {
        if(characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerSprintAndCrouch spint = GetComponent<PlayerSprintAndCrouch>();
            if (spint.SprintValue >= spint.StaminaForJump)
            {
                verticalVelocity = jumpForce;
                spint.Jump();
            }
        }
    }
}
