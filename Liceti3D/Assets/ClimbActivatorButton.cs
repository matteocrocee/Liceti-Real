using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ClimbActivatorButton : MonoBehaviour
{
    public GameObject climbZoneToToggle;
    private Climb_zone climbZoneScript;
    private Renderer buttonRenderer;

    private Vector3 inactiveScale = new Vector3(1f, 0.2f, 1f);   // Sottile orizzontale
    private Vector3 activeScale = new Vector3(0.5f, 1.5f, 0.5f); // Spesso verticale

    private void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;

        buttonRenderer = GetComponent<Renderer>();
        if (buttonRenderer == null)
        {
            Debug.LogWarning("Nessun Renderer trovato sul pulsante. Il colore non sarà aggiornato.");
        }

        if (climbZoneToToggle != null)
        {
            climbZoneScript = climbZoneToToggle.GetComponent<Climb_zone>();
            if (climbZoneScript == null)
            {
                Debug.LogError("Climb_zone script mancante nell'oggetto assegnato.");
            }
            else
            {
                UpdateButtonAppearance(); // Forma e colore iniziali
            }
        }
        else
        {
            Debug.LogError("Climb zone non assegnata!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && climbZoneScript != null)
        {
            climbZoneScript.ToggleZone();
            UpdateButtonAppearance();

            Debug.Log("Climb zone " + (climbZoneScript.isActive ? "attivata" : "disattivata") + " dal pulsante.");
        }
    }

    private void UpdateButtonAppearance()
    {
        // Colore
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = climbZoneScript.isActive ? Color.green : Color.red;
        }

        // Forma
        transform.localScale = climbZoneScript.isActive ? activeScale : inactiveScale;
    }
}
