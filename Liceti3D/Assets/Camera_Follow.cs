using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target;         // Il personaggio da seguire
    public Vector3 offset = new Vector3(0f, 5f, -8f);  // Offset rispetto al personaggio
    public float smoothSpeed = 5f;   // Velocità di follow

    void LateUpdate()
    {
        if (target == null) return;

        // Posizione desiderata basata sull'offset
        Vector3 desiredPosition = target.position + offset;

        // Interpolazione fluida verso la posizione desiderata
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Aggiorna posizione del punto (la camera sarà figlia di questo)
        transform.position = smoothedPosition;

        // Guarda il personaggio (opzionale)
        transform.LookAt(target);
    }
}
