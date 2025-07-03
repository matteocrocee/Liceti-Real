using System.Collections;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{
    public float jumpMultiplier = 2f;       // Moltiplicatore salto
    public float duration = 5f;              // Durata effetto in secondi

    private void OnTriggerEnter(Collider other)
    {
        Personaggio2 player = other.GetComponent<Personaggio2>();
        if (player != null)
        {
            StartCoroutine(ApplyJumpPowerUp(player));
            // Distruggi il power-up
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyJumpPowerUp(Personaggio2 player)
    {
        float originalJumpHeight = player.jumpHeight;
        player.jumpHeight *= jumpMultiplier; // Aumenta salto

        yield return new WaitForSeconds(duration);

        player.jumpHeight = originalJumpHeight; // Ripristina salto
    }
}
