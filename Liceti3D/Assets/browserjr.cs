using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseWithHealth : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 8f;
    public float speed = 3.5f;
    public int maxHits = 3;
    public float raycastLength = 1.5f; // lunghezza del raycast per colpire la testa

    private int currentHits = 0;
    private NavMeshAgent agent;
    private bool isAlive = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.speed = speed;
        else
            Debug.LogError("NavMeshAgent mancante!");
    }

    void Update()
    {
        if (!isAlive || player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position); // resta fermo
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive || collision.gameObject != player.gameObject)
            return;

        // Lancia un raycast dal centro del player verso il basso
        Ray ray = new Ray(player.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastLength))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                // Ha colpito il nemico dall'alto
                HandleHeadHit();
                return;
            }
        }

        // Se non ha colpito la testa, elimina il giocatore
        Destroy(player.gameObject);
        Debug.Log("Il nemico ha eliminato il giocatore.");
    }

    void HandleHeadHit()
    {
        currentHits++;
        Debug.Log("Colpo in testa al nemico! Totale: " + currentHits);

        // Rimbalzo del giocatore
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, 8f, playerRb.velocity.z);
        }

        if (currentHits >= maxHits)
        {
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        Debug.Log("Nemico eliminato dopo 3 colpi in testa!");
        Destroy(gameObject);
    }
}
