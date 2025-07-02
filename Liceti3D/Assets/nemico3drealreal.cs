using UnityEngine;

public class EnemyPatrolAndDamage : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float speed = 3f;
    public int damage = 10;

    private Transform target;

    private void Start()
    {
        target = puntoB; // Inizia andando verso puntoB
    }

    private void Update()
    {
        // Movimento verso il target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Se raggiungo il target, cambio direzione
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            target = (target == puntoA) ? puntoB : puntoA;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Controlla se ha toccato il giocatore (assumo che il giocatore abbia il tag "Player")
        if (collision.gameObject.CompareTag("Player"))
        {
            // Prendo il componente del giocatore che gestisce la salute
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
