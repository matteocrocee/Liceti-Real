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
            Debug.LogError("CharacterController not found! Please add it to the GameObject.");
            enabled = false;
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        respawnPosition = transform.position; // Salva la posizione iniziale come spawn
    }

    void Update()
    {
        if (isDashing)
            return; // Blocca input e movimento normale durante il dash

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

        // Rotazione fluida
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angleVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }

        // Salto e gravità (considera power jump)
        if (characterController.isGrounded)
        {
            velocity.y = -1f;

            if (Input.GetButtonDown("Jump"))
            {
                float jumpPower = powerJumpActive ? jumpHeight * 2f : jumpHeight; // salto potenziato se power-up attivo
                velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Corsa con shift
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : moveSpeed;

        // Movimento complessivo
        Vector3 finalMove = moveDirection * speed;
        finalMove.y = velocity.y;
        characterController.Move(finalMove * Time.deltaTime);

        // Dash istant kill: se power-up attivo, Q fa dash kill, altrimenti no
        if (speedInstantKillActive)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isDashing)
            {
                StartCoroutine(DashKill());
            }
        }
    }

    IEnumerator DashKill()
    {
        isDashing = true;

        Vector3 dashDirection = transform.forward;
        float elapsedTime = 0f;

        if (dashEffect != null)
            dashEffect.SetActive(true);

        while (elapsedTime < dashDuration)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);

            // Controlla collisione con nemici
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 1.0f, enemyLayer);
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out EnemyPatrol enemyScript))
                {
                    enemyScript.Muori();
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (dashEffect != null)
            dashEffect.SetActive(false);

        isDashing = false;
    }

    // PowerUp methods
    public void IniziaSpeedInstantKill(float durata)
    {
        speedInstantKillActive = true;
        StartCoroutine(SpeedInstantKillTimer(durata));
    }

    public void FermaSpeedInstantKill()
    {
        speedInstantKillActive = false;
    }

    private IEnumerator SpeedInstantKillTimer(float durata)
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

    private IEnumerator PowerJumpTimer(float durata)
    {
        yield return new WaitForSeconds(durata);
        FermaPowerJump();
    }

    // Metodo chiamato da Enemy per uccidere il player
    public void Muori()
    {
        // Puoi aggiungere effetti morte qui, es: animazione, suono etc.

        Respawn();
    }

    // Respawn semplice e istantaneo
    private void Respawn()
    {
        // Ferma velocità verticale
        velocity.y = 0f;

        // Teletrasporto alla posizione di spawn
        characterController.enabled = false;  // Disabilita controller prima di spostare
        transform.position = respawnPosition;
        characterController.enabled = true;   // Riabilita controller

        // Se vuoi puoi resettare altri stati o parametri qui
    }

    // (Facoltativo) disegna il raggio dash in editor per debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}
