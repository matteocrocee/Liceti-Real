using UnityEngine;
using System.Collections.Generic;

public class Nemico_script : MonoBehaviour
{
    [Header("Movimento")]
    public List<Transform> patrolPoints;
    public float speed = 3f;
    public float stopDistance = 0.2f;

    [Header("Comportamento")]
    public float headKillThreshold = 0.5f;
    public Transform player;
    public float chaseDistance = 5f;

    [Header("Colori")]
    public Color patrolColor = Color.white;
    public Color chaseColor = Color.red;

    private int currentPointIndex = 0;
    private bool isAlive = true;
    private bool isChasing = false;

    private Rigidbody rb;
    private Renderer[] renderers;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        if (player == null && GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        renderers = GetComponentsInChildren<Renderer>();
        SetColor(patrolColor);
    }

    void Update()
    {
        if (!isAlive || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <= chaseDistance;

        if (isChasing)
        {
            // Inseguimento
            Vector3 chaseDirection = (player.position - transform.position).normalized;
            transform.position += chaseDirection * speed * Time.deltaTime;
            SetColor(chaseColor);
        }
        else
        {
            // Pattugliamento
            SetColor(patrolColor);

            if (patrolPoints.Count == 0) return;

            Transform targetPoint = patrolPoints[currentPointIndex];
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPoint.position) <= stopDistance)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isAlive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb == null) return;

            float playerY = collision.transform.position.y;
            float enemyTopY = transform.position.y + GetComponent<Collider>().bounds.extents.y;

            if (playerY > enemyTopY - headKillThreshold)
            {
                Die();
                playerRb.velocity = new Vector3(playerRb.velocity.x, 8f, playerRb.velocity.z);
            }
            else
            {
                var playerScript = collision.gameObject.GetComponent<Personaggio2>();
                if (playerScript != null)
                {
                    playerScript.Muori();
                }
            }
        }
    }

    void Die()
    {
        isAlive = false;
        Debug.Log("Nemico ucciso saltando in testa!");

        GetComponent<Collider>().enabled = false;
        foreach (Renderer r in renderers)
            r.enabled = false;

        Destroy(gameObject, 1f);
    }

    void SetColor(Color color)
    {
        foreach (Renderer r in renderers)
        {
            if (r.material.HasProperty("_Color"))
                r.material.color = color;
        }
    }
}
