using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 90f; // Rotazione in gradi al secondo
    public AudioClip pickupSound;     // (Facoltativo) suono alla raccolta
    public GameObject pickupEffect;   // (Facoltativo) effetto visivo
    private bool collected = false;

    void Update()
    {
        // Ruota continuamente la moneta
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        // Assicurati che solo il player possa raccoglierla
        if (!collected && other.CompareTag("Player"))
        {
            collected = true;

            // Aggiungi punti o logica al contatore
            CoinCollector.Instance.AddCoin();

            // Effetto visivo opzionale
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Suono opzionale
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Distruggi la moneta
            Destroy(gameObject);
        }
    }
}
