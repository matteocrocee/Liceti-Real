using System;
using System.Collections;
using UnityEngine;

public class Personaggio2 : MonoBehaviour
{
    public float moveSpeed = 7.0f;
    public float runSpeed = 12.0f;
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public LayerMask enemyLayer;
    public GameObject dashEffect;

    public float jumpHeight = 0.56f;
    public float gravity = -9.81f;
    public float rotationSmoothTime = 0.1f;
    public Transform cameraTransform;

    public float fallThresholdY = -10f; // altezza sotto la quale si considera caduto

    private CharacterController characterController;
    private Vector3 velocity;
    private float angleVelocity;
    private bool isDashing = false;

    private bool speedInstantKillActive = false;
    private bool powerJumpActive = false;

    private Vector3 respawnPosition;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController non trovato!");
            enabled = false;
        }

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        respawnPosition = transform.position;
    }

    void Update()
    {
        if (isDashing) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        Vector3 moveDirection = (forward * v + right * h).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angleVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }

        if (characterController.isGrounded)
        {
            velocity.y = -1f;

            if (Input.GetButtonDown("Jump"))
            {
                float jumpPower = powerJumpActive ? jumpHeight * 2f : jumpHeight;
                velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : moveSpeed;

        Vector3 finalMove = moveDirection * speed;
        finalMove.y = velocity.y;
        characterController.Move(finalMove * Time.deltaTime);

        if (speedInstantKillActive && Input.GetKeyDown(KeyCode.Q) && !isDashing)
            StartCoroutine(DashKill());

        // Controlla se è caduto nel vuoto
        if (transform.position.y < fallThresholdY)
        {
            Respawn();
        }
    }

    IEnumerator DashKill()
    {
        isDashing = true;
        Vector3 dashDirection = transform.forward;
        float elapsedTime = 0f;

        if (dashEffect != null) dashEffect.SetActive(true);

        while (elapsedTime < dashDuration)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);

            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 1.0f, enemyLayer);
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out EnemyPatrol enemyScript))
                    enemyScript.Muori();
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (dashEffect != null) dashEffect.SetActive(false);

        isDashing = false;
    }

    public void IniziaSpeedInstantKill(float durata)
    {
        speedInstantKillActive = true;
        StartCoroutine(SpeedInstantKillTimer(durata));
    }

    public void FermaSpeedInstantKill()
    {
        speedInstantKillActive = false;
    }

    IEnumerator SpeedInstantKillTimer(float durata)
    {
        yield return new WaitForSeconds(durata);
        FermaSpeedInstantKill();
    }

    public void IniziaPowerJump(float durata)
    {
        powerJumpActive = true;
        StartCoroutine(PowerJumpTimer(durata));
    }

    public void FermaPowerJump()
    {
        powerJumpActive = false;
    }

    IEnumerator PowerJumpTimer(float durata)
    {
        yield return new WaitForSeconds(durata);
        FermaPowerJump();
    }

    public void Muori()
    {
        Respawn();
    }

    private void Respawn()
    {
        velocity.y = 0f;
        characterController.enabled = false;
        transform.position = respawnPosition;
        characterController.enabled = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }

    internal void TriggerRespawnFromFalling()
    {
        throw new NotImplementedException();
    }
}
