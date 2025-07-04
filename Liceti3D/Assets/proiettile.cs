using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector] public GameObject shooter; // Chi ha sparato il proiettile

    private void OnCollisionEnter(Collision collision)
    {
        // Evita autodistruzione se colpisce se stesso o il nemico che lo ha sparato
        if (collision.gameObject == shooter || collision.gameObject == this.gameObject)
            return;

        // Se colpisce il giocatore → distruggilo
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Giocatore eliminato dal proiettile!");
        }

        // In ogni altro caso → distruggi il proiettile
        Destroy(gameObject);
    }
}
