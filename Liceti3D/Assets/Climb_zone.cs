using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Climb_zone : MonoBehaviour
{
    public Vector3 climbDirection = Vector3.up;
    public bool isActive = false;

    [Header("Muro visivo associato")]
    public GameObject wallVisual;

    private Renderer wallRenderer;

    private Collider _collider;
    private MeshRenderer _renderer;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        if (_collider != null)
            _collider.isTrigger = true;

        _renderer = GetComponent<MeshRenderer>();
        if (_renderer != null)
            _renderer.enabled = false;

        if (wallVisual != null)
        {
            wallRenderer = wallVisual.GetComponent<Renderer>();
            UpdateWallColor(); // imposta colore iniziale
        }

        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            Personaggio2 playerScript = other.GetComponent<Personaggio2>();
            if (playerScript != null)
            {
                playerScript.StartClimbing(climbDirection);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Personaggio2 playerScript = other.GetComponent<Personaggio2>();
            if (playerScript != null)
            {
                playerScript.StopClimbing();
            }
        }
    }

    public void ToggleZone()
    {
        isActive = !isActive;

        UpdateWallColor();

        if (isActive)
        {
            Collider[] hits = Physics.OverlapBox(transform.position, _collider.bounds.extents, Quaternion.identity);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    var playerScript = hit.GetComponent<Personaggio2>();
                    if (playerScript != null)
                    {
                        playerScript.StartClimbing(climbDirection);
                    }
                }
            }
        }
    }

    private void UpdateWallColor()
    {
        if (wallRenderer != null)
        {
            wallRenderer.material.color = isActive ? Color.green : Color.red;
        }
    }
}