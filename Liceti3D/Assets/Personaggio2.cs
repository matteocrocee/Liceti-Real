using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaggio2 : MonoBehaviour
{
    public float moveSpeed = 7.0f;
    public float rotationSpeed = 400.0f;
    public float jumpHeight = 0.56f;
    public float gravity = -9.81f;
    public float rotationSmoothTime = 0.1f;
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 velocity;
    private float currentAngle;
    private float angleVelocity;

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
            transform.position = Vector3.forward * Time.deltaTime * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("premuto S");
            transform.position = Vector3.back * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("premuto A");
            transform.position = Vector3.left * Time.deltaTime * moveSpeed;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("premuto D");
            transform.position = Vector3.right * Time.deltaTime * moveSpeed;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Direzione basata sulla camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Elimina il movimento verticale
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * v + right * h).normalized;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotazione
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Gravità e salto
        if (characterController.isGrounded)
        {
            velocity.y = -1f;

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