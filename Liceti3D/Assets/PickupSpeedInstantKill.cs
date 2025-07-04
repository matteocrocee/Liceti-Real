using UnityEngine;

public class PickupSpeedInstantKill : MonoBehaviour
{
    public float durataPowerUp = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.RaccogliSpeedInstantKill(durataPowerUp);
            Destroy(gameObject);
        }
    }
}
