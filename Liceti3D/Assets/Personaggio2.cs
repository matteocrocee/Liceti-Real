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
    private Vector3 velocity; // Per il salto/gravity
    private float angleVelocity; // Per rotazione fluida

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
        // --- INPUT MOVIMENTO ORIZZONTALE ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // --- MOVIMENTO RELATIVO ALLA CAMERA ---
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * v + right * h).normalized;

        // --- ROTAZIONE FLUIDA ---
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angleVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }

        // --- SALTO E GRAVITÀ ---
        if (characterController.isGrounded)
        {
            velocity.y = -1f; // Tiene il personaggio incollato a terra

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calcolo fisico del salto
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Applica gravità quando in aria
        }

        // --- MOVIMENTO COMPLESSIVO (orizzontale + verticale) ---
        Vector3 finalMove = moveDirection * moveSpeed;
        finalMove.y = velocity.y; // aggiunge movimento verticale
        characterController.Move(finalMove * Time.deltaTime);

        // --- DEBUG (rimosso movimento errato con transform.position) ---
        // Ho rimosso la parte con `transform.position = ...` perché va contro l'uso corretto del CharacterController.
        // Se vuoi debug visivi, puoi usare Debug.DrawRay oppure Debug.Log senza muovere manualmente l'oggetto.
    }
}
