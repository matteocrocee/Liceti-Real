using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;     // Prefab del proiettile
    public Transform firePoint;             // Punto da cui sparare
    public float projectileSpeed = 10f;
    public float fireRate = 2f;             // Tempo tra un colpo e l'altro
    public float shootingDistance = 10f;    // Distanza massima per sparare
    public Transform target;                // Giocatore

    private float fireCooldown = 0f;

    void Update()
    {
        if (target == null) return;

        fireCooldown -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer <= shootingDistance && fireCooldown <= 0f)
        {
            ShootAtPlayer();
            fireCooldown = fireRate;
        }
    }

    void ShootAtPlayer()
    {
        Vector3 spawnPos = firePoint.position + firePoint.forward * 0.5f;
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector3 direction = (target.position - firePoint.position).normalized;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        // Imposta chi ha sparato
        EnemyProjectile projScript = projectile.GetComponent<EnemyProjectile>();
        if (projScript != null)
        {
            projScript.shooter = gameObject;
        }

        // Ignora collisioni col nemico stesso
        Collider[] enemyColliders = GetComponentsInChildren<Collider>();
        Collider projCollider = projectile.GetComponent<Collider>();

        foreach (var col in enemyColliders)
        {
            if (projCollider != null && col != null)
                Physics.IgnoreCollision(projCollider, col);
        }
    }
}
