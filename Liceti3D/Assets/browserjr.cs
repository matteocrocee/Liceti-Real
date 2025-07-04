using UnityEngine;
using UnityEngine.AI;

public class browserjr : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 8f;
    public float speed = 3.5f;
    public int maxHits = 3;
    public float headKillOffset = 0.4f;

    public AudioClip deathSound; // Clip da assegnare
    private AudioSource audioSource;

    private int currentHits = 0;
    private NavMeshAgent agent;
    private bool isAlive = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        if (agent != null)
            agent.speed = speed;
    }

    void Update()
    {
        if (!isAlive || player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
            agent.SetDestination(player.position);
        else
            agent.SetDestination(transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive)
            return;

        if (collision.gameObject == player.gameObject)
        {
            // Controlla se il player ha speedInstantKill attivo
            Personaggio2 p = player.GetComponent<Personaggio2>();
            if (!(p == null || !p.speedInstantKillActive))
            {
                muori();
                return;
            }

            float playerBottomY = player.position.y;
            float enemyTopY = transform.position.y + GetComponent<Collider>().bounds.extents.y;

            if (playerBottomY > enemyTopY + headKillOffset)
            {
                HandleHeadHit();
            }
            else
            {
                Destroy(player.gameObject);
                Debug.Log("Il nemico ha eliminato il giocatore.");
            }
        }
    }

    void HandleHeadHit()
    {
        currentHits++;

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, 8f, playerRb.velocity.z);
        }

        if (currentHits >= maxHits)
        {
            muori();
        }
    }

    void muori()
    {
        isAlive = false;

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Disattiva visivamente e fisicamente il nemico, ma lo distrugge dopo il suono
        GetComponent<Collider>().enabled = false;
        if (agent != null) agent.enabled = false;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        Destroy(gameObject, deathSound.length); // distruggi dopo fine suono
    }
}
