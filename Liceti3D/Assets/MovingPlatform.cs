using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;
    private Vector3 lastPosition;

    private GameObject playerOnPlatform = null;
    private Vector3 playerOriginalScale;

    private Rigidbody platformRb;

    private void Start()
    {
        target = pointB.position;
        lastPosition = transform.position;
        platformRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        platformRb.MovePosition(newPos);

        // Cambio direzione se raggiunto il target
        if (Vector3.Distance(newPos, target) < 0.1f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }

        // Movimento relativo del player (opzionale se MovePosition funziona bene da solo)
        if (playerOnPlatform != null)
        {
            Vector3 delta = newPos - lastPosition;
            playerOnPlatform.transform.position += delta;
        }

        lastPosition = newPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = collision.gameObject;
            playerOriginalScale = playerOnPlatform.transform.localScale; // salva la scala
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform.transform.localScale = playerOriginalScale; // ripristina scala
            playerOnPlatform = null;
        }
    }
}
