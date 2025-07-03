using UnityEngine;

public class PowerUpSpeed : MonoBehaviour
{
    public AudioClip pickupSound;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Personaggio2 player = GameObject.FindWithTag("Player")?.GetComponent<Personaggio2>();
            if (player != null)
            {
                player.RaccogliPowerUpSpeed();
            }

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
