using UnityEngine;

public class PickupSpeedInstantKill : MonoBehaviour
{
    public AudioClip pickupSound;
    public GameObject pickupEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notifica al GameManager che è stato raccolto
            GameManager.Instance.RaccogliPowerUpSpeedInstantKill();

            // Effetto visivo (opzionale)
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            // Suono pickup (opzionale)
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Distruggi il power-up
            Destroy(gameObject);
        }
    }
}
