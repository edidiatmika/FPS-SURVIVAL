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


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        moveDirection = new Vector3(Input.GetAxis(Axis.horizontal), 0f, 
                                     Input.GetAxis(Axis.vertical));

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;

        ApplyGravity();

        characterController.Move(moveDirection);
    }

    void ApplyGravity()
    {

        verticalVelocity -= gravity * Time.deltaTime;

        PlayerJump();
        

        moveDirection.y = verticalVelocity * Time.deltaTime;
    }

    void PlayerJump()
    {
        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
        }
    }
}
