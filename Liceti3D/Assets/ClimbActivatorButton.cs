using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ClimbActivatorButton : MonoBehaviour
{
    public GameObject climbZoneToToggle;
    private Climb_zone climbZoneScript;
    private Renderer buttonRenderer;
    private Renderer climbZoneRenderer;

    private bool alreadyActivated = false;

    private Vector3 inactiveScale = new Vector3(1f, 0.2f, 1f);   // Sottile
    private Vector3 activeScale = new Vector3(0.5f, 1.5f, 0.5f); // Spesso

    private void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        buttonRenderer = GetComponent<Renderer>();

        if (climbZoneToToggle != null)
        {
            climbZoneScript = climbZoneToToggle.GetComponent<Climb_zone>();
            climbZoneRenderer = climbZoneToToggle.GetComponent<Renderer>();

            if (climbZoneScript == null)
            {
                Debug.LogError("Climb_zone script mancante nell'oggetto assegnato.");
            }

            UpdateVisuals(); // Colori e scala iniziali
        }
        else
        {
            Debug.LogError("Climb zone non assegnata!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyActivated) return;

        if (other.CompareTag("Player") && climbZoneScript != null)
        {
            climbZoneScript.ToggleZone(); // Attiva la zona
            alreadyActivated = true;      // Blocca ulteriori attivazioni
            UpdateVisuals();

            Debug.Log("Climb zone attivata dal pulsante.");
        }
    }

    private void UpdateVisuals()
    {
        Color colorToSet = climbZoneScript.isActive ? Color.green : Color.red;

        // Cambia colore del pulsante
        if (buttonRenderer != null)
            buttonRenderer.material.color = colorToSet;

        // Cambia colore della zona
        if (climbZoneRenderer != null)
            climbZoneRenderer.material.color = colorToSet;

        // Cambia forma del pulsante
        transform.localScale = climbZoneScript.isActive ? activeScale : inactiveScale;
    }
}
