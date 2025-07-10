using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    [Header("Punti di movimento")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    [Header("Colore")]
    public Color activeColor = Color.red;
    public Color inactiveColor = Color.green;

    [Header("Soglia di uccisione dall'alto")]
    public float headKillThreshold = 0.5f;

    private bool isActive = true;
    private bool movingToB = true;

    private Renderer[] renderers;
    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        col = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        SetColor(activeColor);
    }

    void Update()
    {
        if (!isActive || pointA == null || pointB == null) return;

        Vector3 target = movingToB ? pointB.position : pointA.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            movingToB = !movingToB;
        }
    }

    public void Deactivate()
    {
        isActive = false;
        SetColor(inactiveColor);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        var player = collision.gameObject;
        var playerRb = player.GetComponent<Rigidbody>();
        var playerScript = player.GetComponent<Personaggio2>();

        if (playerRb == null || playerScript == null) return;

        float playerY = player.transform.position.y;
        float enemyTopY = transform.position.y + col.bounds.extents.y;

        bool isHeadJump = playerY > (enemyTopY - headKillThreshold);

        if (isActive)
        {
            // Se attivo, il nemico uccide il giocatore in ogni caso
            Debug.Log("Giocatore colpito da nemico ATTIVO ¡ú MUORE");
            playerScript.Muori();
        }
        else
        {
            if (isHeadJump)
            {
                Debug.Log("Giocatore ha saltato sulla testa del nemico disattivato ¡ú NEMICO DISTRUTTO");
                playerRb.velocity = new Vector3(playerRb.velocity.x, 8f, playerRb.velocity.z); // Rimbalzo verso l'alto
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Giocatore ha toccato il nemico disattivato ma NON dalla testa ¡ú nessuna azione");
                // Puoi decidere se farlo morire comunque o ignorare
            }
        }
    }

    private void SetColor(Color color)
    {
        foreach (Renderer r in renderers)
        {
            if (r.material.HasProperty("_Color"))
                r.material.color = color;
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}
