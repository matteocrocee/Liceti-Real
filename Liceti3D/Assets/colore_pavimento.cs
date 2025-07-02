using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeEnvironmentColor : MonoBehaviour
{
    public Color newColor = Color.green;

    void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.material.color = newColor;
        }
    }
}