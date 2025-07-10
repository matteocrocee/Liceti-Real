using UnityEngine;

public class OscillatingPlatform : MonoBehaviour
{
    public Transform pointB;
    public float speed = 2f;

    private Vector3 startPoint;
    private bool activated = false;

    private Rigidbody playerRb;
    private Vector3 lastPlatformPosition;

    private void Start()
    {
        startPoint = transform.position;
        lastPlatformPosition = transform.position;
    }

    private void Update()
    {
        if (!activated) return;

        transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointB.position) < 0.05f)
        {
            Vector3 temp = pointB.position;
            pointB.position = startPoint;
            startPoint = temp;
        }
    }

    private void FixedUpdate()
    {
        // Calcola lo spostamento della piattaforma
        Vector3 delta = transform.position - lastPlatformPosition;

        // Muove il player insieme alla piattaforma
        if (playerRb != null)
        {
            playerRb.MovePosition(playerRb.position + new Vector3(delta.x, 0f, delta.z));
        }

        lastPlatformPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!activated)
                activated = true;

            playerRb = collision.rigidbody;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.rigidbody == playerRb)
                playerRb = null;
        }
    }
}
