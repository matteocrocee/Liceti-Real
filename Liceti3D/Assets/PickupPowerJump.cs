using UnityEngine;

public class PickupPowerJump : MonoBehaviour
{
    public float durataPowerUp = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.RaccogliPowerUpJump(durataPowerUp);
            Destroy(gameObject);
        }
    }
}
