using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector] public GameObject shooter;
    public AudioClip impactSound; // Clip da assegnare in Inspector

    private AudioSource audioSource;
    private bool hasHit = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return; // evita più suoni
        hasHit = true;

        if (collision.gameObject == shooter || collision.gameObject == this.gameObject)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Giocatore eliminato dal proiettile!");
        }

        // Riproduci suono di impatto (se c'è un audio source e clip)
        if (audioSource != null && impactSound != null)
        {
            audioSource.PlayOneShot(impactSound);
        }

        // Distruggi dopo breve ritardo così il suono può finire
        Destroy(gameObject, 0.1f);
    }
}
