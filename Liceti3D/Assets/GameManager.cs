using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TMP_Text punteggioText;
    public TMP_Text powerUpText;

    private int punteggio = 0;

    private bool hasSpeedPowerUp = false;
    private bool hasJumpPowerUp = false;

    private float powerUpDuration = 5f;
    private float powerUpTimer = 0f;

    private enum PowerUpType { None, Speed, Jump }
    private PowerUpType activePowerUp = PowerUpType.None;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AggiornaPunteggioUI();
        powerUpText.text = "";
    }

    void Update()
    {
        if (hasSpeedPowerUp)
        {
            powerUpText.text = "Premi E per attivare Speed!";

            if (Input.GetKeyDown(KeyCode.E))
            {
                AttivaPowerUpSpeed();
            }
        }
        else if (hasJumpPowerUp)
        {
            powerUpText.text = "Premi F per attivare Jump!";

            if (Input.GetKeyDown(KeyCode.F))
            {
                AttivaPowerUpJump();
            }
        }

        if (powerUpTimer > 0f)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                powerUpTimer = 0f;
                DisattivaPowerUp();
            }
        }
    }

    public void AggiungiPunti(int valore)
    {
        punteggio += valore;
        AggiornaPunteggioUI();
    }

    private void AggiornaPunteggioUI()
    {
        if (punteggioText != null)
            punteggioText.text = "Punteggio: " + punteggio;
    }

    public void RaccogliPowerUpSpeed()
    {
        hasSpeedPowerUp = true;
    }

    public void RaccogliPowerUpJump()
    {
        hasJumpPowerUp = true;
    }

    private void AttivaPowerUpSpeed()
    {
        Debug.Log("Power-up Speed attivato!");
        hasSpeedPowerUp = false;
        powerUpText.text = "";
        powerUpTimer = powerUpDuration;
        activePowerUp = PowerUpType.Speed;

        Personaggio2 player = FindObjectOfType<Personaggio2>();
        if (player != null)
        {
            player.AttivaSpeedBoost(powerUpDuration);
        }
    }

    private void AttivaPowerUpJump()
    {
        Debug.Log("Power-up Jump attivato!");
        hasJumpPowerUp = false;
        powerUpText.text = "";
        powerUpTimer = powerUpDuration;
        activePowerUp = PowerUpType.Jump;

        Personaggio2 player = FindObjectOfType<Personaggio2>();
        if (player != null)
        {
            player.AttivaJumpBoost(powerUpDuration);
        }
    }

    private void DisattivaPowerUp()
    {
        Debug.Log("Power-up terminato.");
        powerUpText.text = "";
        activePowerUp = PowerUpType.None;

        Personaggio2 player = FindObjectOfType<Personaggio2>();
        if (player != null)
        {
            player.DisattivaPowerUps();
        }
    }
}
