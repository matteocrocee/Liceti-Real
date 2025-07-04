using UnityEngine;

public class Coin : MonoBehaviour
{
    public int valore = 1;
    public AudioClip coinSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AggiungiPunti(valore);

            if (coinSound != null)
                AudioSource.PlayClipAtPoint(coinSound, transform.position);

            Destroy(gameObject);
        }
    }
}
