using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float damageCooldown = 1f;
    public int damage = 10;
    public GameObject esplosionePrefab;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private float lastDamageTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Aggiungi un NavMeshAgent al nemico!");
            enabled = false;
            return;
        }

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Se è vicino al waypoint corrente, passa al prossimo
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime > damageCooldown)
            {
                collision.gameObject.SendMessage("Muori", SendMessageOptions.DontRequireReceiver);
                lastDamageTime = Time.time;
            }
        }
    }

    public void Muori()
    {
        if (esplosionePrefab != null)
        {
            Instantiate(esplosionePrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
