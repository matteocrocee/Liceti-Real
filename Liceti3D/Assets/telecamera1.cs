using UnityEngine;

public class telecamera1 : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float sensitivity = 3f;
    public float distance = 4f;
    public float minPitch = -30f;
    public float maxPitch = 60f;
    public float rotationSmoothSpeed = 5f;

    private float yaw = 0f;
    private float pitch = 10f;

    void LateUpdate()
    {
        // ✅ Evita errori se il target è stato distrutto
        if (target == null)
            return;

        // Input mouse
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Calcola rotazione della camera
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0);

        // Posizione desiderata
        Vector3 desiredPosition = target.position + cameraRotation * new Vector3(0, 0, -distance) + Vector3.up * offset.y;

        // Imposta posizione e guarda il target
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * offset.y);

        // Rotazione morbida del personaggio verso la direzione della telecamera
        Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
        target.rotation = Quaternion.Slerp(target.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
    }

    // ✅ Permette di aggiornare il target in modo dinamico (es. dopo respawn)
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
