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
        // Se ho un power-up Speed da attivare
        if (hasSpeedPowerUp)
        {
            powerUpText.text = "Premi E per attivare Speed!";

            if (Input.GetKeyDown(KeyCode.E))
            {
                AttivaPowerUpSpeed();
            }
        }
        // Se ho un power-up Jump da attivare
        else if (hasJumpPowerUp)
        {
            powerUpText.text = "Premi F per attivare Jump!";

            if (Input.GetKeyDown(KeyCode.F))
            {
                AttivaPowerUpJump();
            }
        }

        // Gestione durata power-up attivi
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

    // Metodo da chiamare quando si raccoglie un power-up Speed nel gioco
    public void RaccogliPowerUpSpeed()
    {
        hasSpeedPowerUp = true;
    }

    // Metodo da chiamare quando si raccoglie un power-up Jump nel gioco
    public void RaccogliPowerUpJump()
    {
        hasJumpPowerUp = true;
    }

    // Attiva power-up Speed, parte durata e notifica personaggio
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

    // Attiva power-up Jump, parte durata e notifica personaggio
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

    // Disattiva power-up e resetta stato
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
