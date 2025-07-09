using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;
    private Vector3 lastPosition;
    private Rigidbody playerRb;

    private void Start()
    {
        target = pointB.position;
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Muove la piattaforma verso il target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Quando raggiunge il target, lo cambia
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }

    private void FixedUpdate()
    {
        // Calcola la velocità della piattaforma
        Vector3 platformVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;

        // Se c'è un Rigidbody del player, aggiungigli la velocità della piattaforma
        if (playerRb != null)
        {
            playerRb.velocity += new Vector3(platformVelocity.x, 0f, platformVelocity.z);
        }

        // Aggiorna la posizione precedente della piattaforma
        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            playerRb = collision.rigidbody;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            playerRb = null;
        }
    }
}
