using System;
using UnityEngine;

public class Personaggio2 : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float sprintSpeed = 15f;
    private float currentSpeed;
    private bool isSpeedBoosted = false;
    private float speedBoostEndTime = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        Muovi();

        if (isSpeedBoosted && Time.time > speedBoostEndTime)
        {
            DisattivaSpeedBoost();
        }
    }

    private void Muovi()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        rb.MovePosition(transform.position + move * currentSpeed * Time.deltaTime);

        if (!isSpeedBoosted) // solo corsa normale con shift se non è attivo powerup speed
        {
            bool running = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            CorsaNormale(running);
        }
    }

    public void CorsaNormale(bool correndo)
    {
        if (correndo)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
    }

    public void AttivaSpeedBoost(float durata)
    {
        isSpeedBoosted = true;
        speedBoostEndTime = Time.time + durata;
        currentSpeed = sprintSpeed * 1.5f; // esempio speed boost +50%
    }

    public void DisattivaSpeedBoost()
    {
        isSpeedBoosted = false;
        currentSpeed = moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Nemico"))
        {
            if (isSpeedBoosted)
            {
                // Distruggi nemico se in sprint boost
                Destroy(collision.gameObject);
            }
            else
            {
                // Qui metti il danno a te stesso, es. vita -1, ecc.
                Debug.Log("Hai preso danno dal nemico!");
            }
        }
    }

    internal void RaccogliPowerUpSpeed()
    {
        throw new NotImplementedException();
    }
}
