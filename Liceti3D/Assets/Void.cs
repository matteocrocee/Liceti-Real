using UnityEngine;

public class Void : MonoBehaviour
{
    public Transform teleportDestination; // Punto in cui riportare il giocatore

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 🔹 Rende invisibile la piattaforma (nasconde il mesh renderer)
            GetComponent<Renderer>().enabled = false;

            // 🔹 Teletrasporta il giocatore alla destinazione
            collision.gameObject.transform.position = teleportDestination.position;

            // 🔹 (Opzionale) resetta la velocità per evitare rimbalzi strani
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
