using UnityEngine;
using TMPro;

public class script_speedkill : MonoBehaviour
{
    public TMP_Text powerUpText;
    public float powerUpDuration = 5f;

    private bool hasPowerUp = false;
    private bool isPowerUpActive = false;
    private float powerUpTimer = 0f;

    private Personaggio2 player;

    void Start()
    {
        powerUpText.text = "";
        player = FindObjectOfType<Personaggio2>();
    }

    void Update()
    {
        if (hasPowerUp && !isPowerUpActive)
        {
            powerUpText.text = "Premi E per attivare Speed Instant Kill!";
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttivaPowerUp();
            }
        }

        if (isPowerUpActive)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                DisattivaPowerUp();
            }
        }
    }

    public void RaccogliPowerUp()
    {
        hasPowerUp = true;
        powerUpText.text = "Premi E per attivare Speed Instant Kill!";
    }

    private void AttivaPowerUp()
    {
        if (player == null) return;

        isPowerUpActive = true;
        hasPowerUp = false;
        powerUpText.text = "Speed Instant Kill attivo!";
        powerUpTimer = powerUpDuration;

        player.IniziaSpeedInstantKill(powerUpDuration);
    }

    private void DisattivaPowerUp()
    {
        isPowerUpActive = false;
        powerUpText.text = "";
        player.FermaSpeedInstantKill();
    }
}

