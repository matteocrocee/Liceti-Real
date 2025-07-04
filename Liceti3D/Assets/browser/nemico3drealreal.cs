using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3f;
    public GameObject esplosionePrefab;

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Personaggio2 player = other.GetComponent<Personaggio2>();
            if (player != null)
            {
                player.Muori();
            }
        }
    }

    public void Muori()
    {
        if (esplosionePrefab != null)
            Instantiate(esplosionePrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
