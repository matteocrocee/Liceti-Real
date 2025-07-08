using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    public bool isActivated = false;
    public Renderer rend;

    [Header("Colore Stato")]
    public Color offColor = Color.red;
    public Color onColor = Color.green;

    [Header("Riferimento al Manager")]
    public SwitchManager manager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        SetColor(offColor);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            isActivated = true;
            SetColor(onColor);
            if (manager != null)
                manager.NotifySwitchActivated();
        }
    }

    void SetColor(Color color)
    {
        if (rend != null && rend.material.HasProperty("_Color"))
            rend.material.color = color;
    }
}
