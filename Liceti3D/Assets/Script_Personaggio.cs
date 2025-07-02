using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Personaggio : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 400.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Vector3 velocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController not found! Please add it to the GameObject.");
            enabled = false;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;

        // Debug con tasti specifici
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("Premuto W");
        }

        // Rotazione
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Gravità e salto
        if (characterController.isGrounded)
        {
            velocity.y = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        movement.y = velocity.y * Time.deltaTime;

        characterController.Move(movement);
    }
}