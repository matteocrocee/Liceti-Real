using UnityEngine;
using UnityEngine.AI;

public class NPCMovementRandomArea : MonoBehaviour
{
    public Transform areaC; // L'area all'interno della quale si muove l'NCP
    public float waitTime = 2f; // Tempo di attesa prima di cercare un'altra posizione

    private NavMeshAgent agent;
    private float timer;
    private Vector3 targetPosition;
    private Bounds areaBounds;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Ottieni i limiti dell'area C tramite il suo BoxCollider
        BoxCollider box = areaC.GetComponent<BoxCollider>();
        if (box != null)
        {
            areaBounds = box.bounds;
        }
        else
        {
            Debug.LogError("L'area C deve avere un BoxCollider!");
        }

        SetNewRandomDestination();
    }

    private void Update()
    {
        // Se ha raggiunto la destinazione
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                SetNewRandomDestination();
                timer = 0f;
            }
        }
    }

    void SetNewRandomDestination()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(areaBounds.min.x, areaBounds.max.x),
            transform.position.y, // Mantiene la Y attuale (utile se sei su un terreno piatto)
            Random.Range(areaBounds.min.z, areaBounds.max.z)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
