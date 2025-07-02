using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colore : MonoBehaviour
{

    public class ChangeColor : MonoBehaviour
    {
        public Color newColor = Color.red; // Colore da assegnare nel pannello

        void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
            else
            {
                Debug.LogWarning("Nessun Renderer trovato su " + gameObject.name);
            }
        }
    }
}