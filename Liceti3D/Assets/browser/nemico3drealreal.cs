using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;      // Array di punti (A,B,C,D,E) da assegnare dall'Inspector
    public float speed = 3f;           // Velocità di movimento
    public int damage = 10;            // Danno inflitto al giocatore
    public float damageCooldown = 1f;  // Tempo minimo tra i danni consecutivi

    private int currentWaypointIndex = 0;
    private float lastDamageTime = 0f;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Movimento verso il waypoint corrente
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Se vicino al waypoint, passa al prossimo
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Controlla cooldown per non infliggere danno troppo frequentemente
            if (Time.time - lastDamageTime > damageCooldown)
            {
                // Assumiamo che il Player abbia uno script con metodo TakeDamage(int)
                collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                lastDamageTime = Time.time;
            }
        }
    }
}
