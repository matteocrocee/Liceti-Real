using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    public Transform player;         // Riferimento al giocatore da assegnare nell'Inspector
    public float speed = 5f;         // Velocit� di inseguimento
    public float jumpForce = 7f;     // Forza del salto
    public float groundCheckDistance = 0.2f;  // Distanza per controllare se � a terra
    public LayerMask groundLayer;    // Layer del terreno

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (player == null)
            Debug.LogWarning("Player non assegnato in EnemyChaser");
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position);
        direction.y = 0;  // Muovi solo in orizzontale
        direction.Normalize();

        // Rotazione verso il giocatore
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

        // Movimento avanti
        Vector3 move = direction * speed;
        Vector3 velocity = rb.velocity;
        velocity.x = move.x;
        velocity.z = move.z;
        rb.velocity = velocity;

        // Salto se a terra e vicino al giocatore (ad esempio distanza < 3)
        if (IsGrounded() && Vector3.Distance(transform.position, player.position) < 3f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Qui distruggi o elimina il player
            // Esempio: distruggi il giocatore
            Destroy(collision.gameObject);

            // Oppure puoi chiamare un metodo di gestione morte player, tipo:
            // collision.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        }
    }
}
