using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public Vector3 initialForce = new Vector3(0, 0, 10f);

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Salva posizione e rotazione iniziale
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Applica una forza iniziale
        rb.AddForce(initialForce, ForceMode.Impulse);
    }

    void Update()
    {
        // Salto con spazio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }

        // Reset con il tasto "O"
        if (Input.GetKeyDown(KeyCode.O))
        {
            ResetObject();
        }
    }

    void ResetObject()
    {
        // Disabilita momentaneamente la fisica per evitare comportamenti strani
        rb.isKinematic = false;

        // Reset posizione e rotazione
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset velocità
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Riabilita la fisica
        rb.isKinematic = false;
    }
}
