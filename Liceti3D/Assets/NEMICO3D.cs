using UnityEngine;

public class NEMICO3D : MonoBehaviour
{
    public float moveSpeed = 3f;                // Velocità movimento
    public float jumpForce = 5f;                // Forza salto
    public float detectionRange = 5f;           // Distanza per iniziare inseguimento
    public float stopChaseRange = 7f;           // Distanza per smettere inseguimento
    public int damage = 10;                     // Danno al giocatore

    private Rigidbody rb;
    private Transform player;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            ChasePlayer();

            // Salto casuale per dinamismo (opzionale)
            if (isGrounded && Random.value < 0.01f)
            {
                Jump();
            }
        }
        else if (distance > stopChaseRange)
        {
            // Ferma il nemico (componente orizzontale)
            Vector3 vel = rb.velocity;
            vel.x = 0;
            vel.z = 0;
            rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
        }
    }

    void ChasePlayer()
    {
        // Calcola direzione verso il giocatore (sul piano XZ)
        Vector3 direction = (player.position - transform.position);
        direction.y = 0; // non spostarsi in verticale

        direction.Normalize();

        Vector3 moveVelocity = direction * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

        // Ruota il nemico verso il giocatore
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check se il nemico è a terra (terreno taggato "Ground")
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Se tocca il giocatore infligge danno
        if (collision.gameObject.CompareTag("Player"))
        {
            // Esempio: collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Debug.Log("Nemico colpisce il giocatore: danno " + damage);
        }
    }
}
