using UnityEngine;
using UnityEngine.AI;  // Per usare il NavMeshAgent (facilita il movimento)

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Riferimento al giocatore
    public float detectionRadius = 10f; // Raggio di rilevamento del giocatore
    public Transform[] waypoints; // Punti del percorso da seguire
    public float waypointTolerance = 0.5f; // Distanza minima per considerare il waypoint raggiunto
    public float speed = 3.5f; // Velocità del nemico

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Aggiungi un NavMeshAgent al nemico!");
        }
        agent.speed = speed;
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Insegue il giocatore
            agent.SetDestination(player.position);
        }
        else
        {
            // Segue il percorso predefinito
            Patrol();
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0)
            return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < waypointTolerance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; // Riparte dal primo punto
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            // Elimina il giocatore (per esempio distruggi)
            Destroy(player.gameObject);

            // Qui puoi aggiungere ulteriori logiche come Game Over o ricaricare scena
            Debug.Log("Giocatore eliminato!");
        }
    }
}
