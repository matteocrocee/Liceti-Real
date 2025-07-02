using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1.3f;
    public int damage = 10;
    public Transform player;
    private float nextAttackTime;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        Debug.Log("Nemico attacca!");

        // Se il player ha uno script con una funzione TakeDamage(int)
       
    }
}
