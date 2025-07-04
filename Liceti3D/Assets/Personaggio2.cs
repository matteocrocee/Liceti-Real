using System.Collections;
using UnityEngine;

public class Personaggio2 : MonoBehaviour
{
    public float moveSpeed = 7.0f;
    public float runSpeed = 12.0f; // Velocità durante la corsa
    public float rotationSpeed = 400.0f;
    public float jumpHeight = 0.56f;
    public float gravity = -9.81f;
    public float rotationSmoothTime = 0.1f;
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 velocity;
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
            velocity.y = -1f;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // --- CORSA CON SHIFT ---
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : moveSpeed;

        // --- MOVIMENTO COMPLESSIVO ---
        Vector3 finalMove = moveDirection * speed;
        finalMove.y = velocity.y;
        characterController.Move(finalMove * Time.deltaTime);
    }
}

 
