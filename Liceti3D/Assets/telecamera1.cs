using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class telecamera1 : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float sensitivity = 3f;
    public float distance = 4f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw = 0f;
    private float pitch = 10f;

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * new Vector3(0, 0, -distance) + Vector3.up * offset.y;

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}