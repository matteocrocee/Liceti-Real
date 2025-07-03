using UnityEngine;

public class PowerUpSpeed : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Qui chiamo il metodo corretto del GameManager per segnalare la raccolta del power-up speed
            GameManager.Instance.RaccogliPowerUpSpeed();

            // Suono di raccolta power-up (se assegnato)
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Distruggo l'oggetto power-up raccolto
            Destroy(gameObject);
        }
    }
}
