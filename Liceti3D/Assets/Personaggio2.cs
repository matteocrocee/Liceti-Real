using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Personaggio2 : MonoBehaviour
{
    [Header("Movimento")]
    public float moveForce = 10f;
    public float maxSpeed = 8f;
    public float brakingForce = 5f;
    public float rotationTorque = 10f;

    [Header("Salto")]
    public float jumpForce = 5f;
    public float gravityMultiplier = 2f;

    [Header("Dash")]
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public LayerMask enemyLayer;
    public GameObject dashEffect;

    [Header("Power-Up")]
    public bool speedInstantKillActive = false;
    public bool powerJumpActive = false;

    [Header("Altre impostazioni")]
    public float fallThresholdY = -10f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isDashing = false;
    private Vector3 respawnPosition;

    private bool isClimbing = false;
    private Vector3 climbDirection;
    private float climbSpeed = 5f; // Puoi regolare la velocità se vuoi

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.maxAngularVelocity = 100f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        respawnPosition = transform.position;
    }

    public void StartClimbing(Vector3 direction)
    {
        isClimbing = true;
        climbDirection = direction;
        rb.useGravity = false;
        rb.velocity = Vector3.zero; // ferma il movimento verticale
        Debug.Log("Modalità arrampicata attivata");
    }

    public void StopClimbing()
    {
        isClimbing = false;
        rb.useGravity = true;
        Debug.Log("Modalità arrampicata disattivata");
    }

    void Update()
    {
        // Controllo dell'arrampicata
        if (isClimbing)
        {
            float h = Input.GetAxis("Horizontal"); // A/D
            float v = Input.GetAxis("Vertical");   // W/S

            // Vettore UP: direzione verticale del muro (già impostata)
            Vector3 up = climbDirection.normalized;

            // Normale del muro (perpendicolare al piano di arrampicata)
            Vector3 wallNormal = -climbDirection;

            // Se la normale e la direzione verticale coincidono, il Cross fallisce — quindi usiamo un fallback
            Vector3 reference = Vector3.right;
            if (Mathf.Abs(Vector3.Dot(up, reference)) > 0.99f)
            {
                reference = Vector3.forward;
            }

            // Calcolo l'asse destro rispetto al piano del muro
            Vector3 right = -Vector3.Cross(up, reference).normalized;

            // Ricalcolo la normale con cross inverso per coerenza
            wallNormal = Vector3.Cross(right, up).normalized;

            // Movimento combinato su/giù e sinistra/destra
            Vector3 climbInput = (up * v + right * h).normalized;

            rb.MovePosition(transform.position + climbInput * climbSpeed * Time.deltaTime);

            // Uscita dalla modalità arrampicata
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopClimbing();
            }

            return;
        }




        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded && !isDashing)
        {
            float jumpPower = powerJumpActive ? jumpForce * 2f : jumpForce;
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        // Dash
        if (speedInstantKillActive && Input.GetKeyDown(KeyCode.Q) && !isDashing)
        {
            StartCoroutine(DashKill());
        }

        // Caduta nel vuoto
        if (transform.position.y < fallThresholdY)
        {
            Respawn();
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        if (isClimbing)
        {
            // Ignora tutto il resto se stai arrampicando
            return;
        }

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (input.magnitude > 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (camForward * input.z + camRight * input.x).normalized;

            // Aggiunge forza di movimento
            if (rb.velocity.magnitude < maxSpeed)
                rb.AddForce(moveDir * moveForce, ForceMode.Acceleration);

            // Applica torque per rotazione realistica
            Vector3 torqueDir = Vector3.Cross(Vector3.up, moveDir);
            rb.AddTorque(torqueDir * rotationTorque, ForceMode.Acceleration);
        }
        else
        {
            // Rallentamento progressivo
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Vector3 braking = -horizontalVelocity.normalized * brakingForce * Time.fixedDeltaTime;

            if (horizontalVelocity.magnitude > 0.1f)
                rb.AddForce(braking, ForceMode.VelocityChange);
            else
                rb.velocity = new Vector3(0, rb.velocity.y, 0); // blocco finale
        }

        // Gravità aggiuntiva
        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);
        }
    }

    IEnumerator DashKill()
    {
        isDashing = true;
        Vector3 dashDir = rb.velocity.normalized;
        if (dashDir == Vector3.zero) dashDir = transform.forward;

        if (dashEffect != null) dashEffect.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            rb.velocity = dashDir * dashSpeed;

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

        if (dashEffect != null) dashEffect.SetActive(false);
        isDashing = false;
    }

    public void IniziaSpeedInstantKill(float durata)
    {
        speedInstantKillActive = true;
        StartCoroutine(SpeedInstantKillTimer(durata));
    }

    IEnumerator SpeedInstantKillTimer(float durata)
    {
        yield return new WaitForSeconds(durata);
        FermaSpeedInstantKill();
    }

    public void FermaSpeedInstantKill()
    {
        speedInstantKillActive = false;
    }

    public void IniziaPowerJump(float durata)
    {
        powerJumpActive = true;
        StartCoroutine(PowerJumpTimer(durata));
    }

    IEnumerator PowerJumpTimer(float durata)
    {
        yield return new WaitForSeconds(durata);
        FermaPowerJump();
    }

    public void FermaPowerJump()
    {
        powerJumpActive = false;
    }

    public void Muori()
    {
        Respawn();
    }

    private void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.position = respawnPosition;
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }

    internal void TriggerRespawnFromFalling()
    {
        Respawn(); // eventualmente puoi personalizzarlo
    }
}
