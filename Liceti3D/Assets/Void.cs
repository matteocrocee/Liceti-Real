using UnityEngine;

public class Void : MonoBehaviour
{
    public AudioClip voidFallSound; // Suono da riprodurre quando cade (opzionale)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Personaggio2 p = other.GetComponent<Personaggio2>();
            if (p != null)
            {
                // (Opzionale) Suono caduta
                if (voidFallSound != null)
                    AudioSource.PlayClipAtPoint(voidFallSound, p.transform.position);

                Debug.Log("Il giocatore è caduto nel vuoto!");
                p.Muori(); // Respawn istantaneo
            }
        }
    }
}
