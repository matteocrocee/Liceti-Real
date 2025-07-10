using UnityEngine;

public class EnemyButton : MonoBehaviour
{
    public EnemyPatroller enemyToDisable;

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (other.CompareTag("Player"))
        {
            enemyToDisable.Deactivate();
            used = true;
        }
    }
}
