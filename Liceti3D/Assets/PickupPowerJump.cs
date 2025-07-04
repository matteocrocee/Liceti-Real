using UnityEngine;

public class PickupPowerJump : MonoBehaviour
{
    public AudioClip pickupSound;
    public GameObject pickupEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.RaccogliPowerUpJump();

            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}
