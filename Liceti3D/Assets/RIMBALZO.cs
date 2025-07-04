using UnityEngine;

public class RIMBALZO : MonoBehaviour
{
    public float bounceForce = 20f;  // Forza con cui il giocatore viene spinto verso l'alto

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se l'oggetto che ha toccato la piattaforma ha il tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Resetta la velocità verticale per evitare accumuli strani
                Vector3 velocity = playerRb.velocity;
                velocity.y = 0;

                // Applica la forza verso l'alto
                playerRb.velocity = velocity;
                playerRb.AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);
            }
        }
    }
}
