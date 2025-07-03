using UnityEngine;

public class Coin : MonoBehaviour
{
    public int valore = 1; // valore della moneta

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AggiungiPunti(valore); // 👈 Chiama il GameManager

            Destroy(gameObject); // distruggi la moneta
        }
    }
}
