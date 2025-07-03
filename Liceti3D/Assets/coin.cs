using UnityEngine;

public class Coin : MonoBehaviour
{
    public int valore = 1;
    public AudioClip coinSound; // ← assegnerai questo suono nell’Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aggiunge punti
            GameManager.Instance.AggiungiPunti(valore);

            // Riproduce il suono della moneta
            if (coinSound != null)
            {
                AudioSource.PlayClipAtPoint(coinSound, transform.position);
            }

            // Distrugge la moneta
            Destroy(gameObject);
        }
    }
}
