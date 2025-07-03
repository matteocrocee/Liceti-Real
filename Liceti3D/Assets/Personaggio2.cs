using System.Collections;
using UnityEngine;

public class Personaggio2 : MonoBehaviour
{
    public float moveSpeed = 7.0f;
    public float boostedSpeed = 12.0f;
    public float sprintSpeed = 15.0f;        // Velocità corsa con Shift
    public float rotationSpeed = 400.0f;
    public float jumpHeight = 0.56f;
    public float boostedJumpHeight = 1.2f;
    public float gravity = -9.81f;
    public float rotationSmoothTime = 0.1f;
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 velocity;
    private float angleVelocity;

    private bool isSpeedBoosted = false;
    private bool isJumpBoosted = false;

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
        // Input movimento orizzontale
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // Movimento relativo alla camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * v + right * h).normalized;

        // Rotazione fluida verso direzione movimento
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angleVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }

        // Salto e gravità
        if (characterController.isGrounded)
        {
            velocity.y = -1f; // Incolla a terra

            if (Input.GetButtonDown("Jump"))
            {
                float currentJumpHeight = isJumpBoosted ? boostedJumpHeight : jumpHeight;
                velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Calcolo velocità base (normal / speed boost)
        float baseSpeed = isSpeedBoosted ? boostedSpeed : moveSpeed;
        float speed = baseSpeed;

        // Sprint con Left Shift solo se stai muovendo
        if (Input.GetKey(KeyCode.LeftShift) && moveDirection.magnitude > 0.1f)
        {
            speed = sprintSpeed;
        }

        // Movimento finale con velocità e gravità
        Vector3 finalMove = moveDirection * speed;
        finalMove.y = velocity.y;
        characterController.Move(finalMove * Time.deltaTime);
    }

    // Attiva Speed Boost per durata
    public void AttivaSpeedBoost(float durata)
    {
        if (!isSpeedBoosted)
        {
            StartCoroutine(SpeedBoostRoutine(durata));
        }
    }

    private IEnumerator SpeedBoostRoutine(float durata)
    {
        isSpeedBoosted = true;
        Debug.Log("Speed Boost attivato!");
        yield return new WaitForSeconds(durata);
        isSpeedBoosted = false;
        Debug.Log("Speed Boost terminato!");
    }

    // Attiva Jump Boost per durata
    public void AttivaJumpBoost(float durata)
    {
        if (!isJumpBoosted)
        {
            StartCoroutine(JumpBoostRoutine(durata));
        }
    }

    private IEnumerator JumpBoostRoutine(float durata)
    {
        isJumpBoosted = true;
        Debug.Log("Jump Boost attivato!");
        yield return new WaitForSeconds(durata);
        isJumpBoosted = false;
        Debug.Log("Jump Boost terminato!");
    }

    // Disattiva tutti i power-up (speed e jump)
    public void DisattivaPowerUps()
    {
        isSpeedBoosted = false;
        isJumpBoosted = false;
        Debug.Log("Tutti i power-up disattivati!");
    }
}
