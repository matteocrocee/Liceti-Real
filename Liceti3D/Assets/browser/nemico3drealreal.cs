using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;          // Punti da seguire
    public float waypointTolerance = 0.5f; // Distanza minima per considerare un punto raggiunto
    public float speed = 3.5f;             // Velocità del nemico
    public Transform player;               // Riferimento al giocatore

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Aggiungi un NavMeshAgent al nemico!");
            return;
        }

        agent.speed = speed;

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        if (!agent.pathPending && agent.remainingDistance < waypointTolerance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            Destroy(player.gameObject); // Elimina il giocatore
            Debug.Log("Giocatore eliminato dal nemico!");
        }
    }
}
