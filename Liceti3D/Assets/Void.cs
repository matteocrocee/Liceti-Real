using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Void : MonoBehaviour
{
    private void Start()
    {
        // Rende invisibile il Mesh Renderer (se presente)
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Assicura che il collider sia trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogError("Nessun Collider trovato sulla piattaforma!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Personaggio2 player = other.GetComponent<Personaggio2>();
            if (player != null)
            {
                player.TriggerRespawnFromFalling();
            }
        }
    }
}
