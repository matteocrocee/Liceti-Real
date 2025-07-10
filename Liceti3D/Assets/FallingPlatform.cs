using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 0.5f;
    public float fallSpeed = 8f;
    public float respawnDelay = 2f;

    private Vector3 startPoint;
    private Rigidbody playerRb;
    private Renderer rend;
    private Color originalColor;
    private bool hasFallen = false;
    private Vector3 lastPosition;

    private void Start()
    {
        startPoint = transform.position;
        lastPosition = transform.position;
        rend = GetComponent<Renderer>();

        if (rend != null)
            originalColor = rend.material.color;
    }

    private void FixedUpdate()
    {
        Vector3 platformVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;

        if (playerRb != null)
        {
            playerRb.velocity += new Vector3(platformVelocity.x, 0f, platformVelocity.z);
        }

        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasFallen) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = collision.rigidbody;

            if (rend != null)
                rend.material.color = Color.red;

            Invoke(nameof(StartFalling), fallDelay);
            hasFallen = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = null;
        }
    }

    private void StartFalling()
    {
        StartCoroutine(FallAndRespawn());
    }

    private IEnumerator FallAndRespawn()
    {
        float timer = 0f;

        while (timer < respawnDelay)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        // Respawn istantaneo
        transform.position = startPoint;
        hasFallen = false;

        if (rend != null)
            rend.material.color = originalColor;
    }
}
