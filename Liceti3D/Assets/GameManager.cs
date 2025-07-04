using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TMP_Text punteggioText;
    public TMP_Text powerUpText;  // Testo per power-up

    private int punteggio = 0;

    private bool hasSpeedInstantKill = false;
    private bool speedInstantKillActive = false;

    private bool hasPowerJump = false;
    private bool powerJumpActive = false;

    private float powerUpDuration = 5f;
    private float powerUpTimer = 0f;

    private Personaggio2 player;

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
        player = FindObjectOfType<Personaggio2>();
    }

    void Update()
    {
        // Gestione Speed Instant Kill
        if (hasSpeedInstantKill && !speedInstantKillActive)
        {
            powerUpText.text = "Premi E per attivare Speed Instant Kill!";
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttivaPowerUpSpeedInstantKill();
            }
        }

        if (speedInstantKillActive)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                DisattivaPowerUpSpeedInstantKill();
            }
        }

        // Gestione Power Jump
        else if (hasPowerJump && !powerJumpActive)
        {
            powerUpText.text = "Premi E per attivare Power Jump!";
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttivaPowerUpJump();
            }
        }

        if (powerJumpActive)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                DisattivaPowerUpJump();
            }
        }

        // Se nessun power-up attivo o pronto, pulisco il testo
        if (!hasSpeedInstantKill && !speedInstantKillActive && !hasPowerJump && !powerJumpActive)
        {
            powerUpText.text = "";
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
            punteggioText.text = "Coin: " + punteggio;
    }

    // Power-Up Speed Instant Kill
    public void RaccogliPowerUpSpeedInstantKill()
    {
        hasSpeedInstantKill = true;
        powerUpText.text = "Premi E per attivare Speed Instant Kill!";
    }

    private void AttivaPowerUpSpeedInstantKill()
    {
        if (player == null) return;

        speedInstantKillActive = true;
        hasSpeedInstantKill = false;
        powerUpText.text = "Speed Instant Kill attivo!";
        powerUpTimer = powerUpDuration;

        player.IniziaSpeedInstantKill(powerUpDuration);
    }

    private void DisattivaPowerUpSpeedInstantKill()
    {
        speedInstantKillActive = false;
        powerUpText.text = "";
        player.FermaSpeedInstantKill();
    }

    // Power-Up Jump
    public void RaccogliPowerUpJump()
    {
        hasPowerJump = true;
        powerUpText.text = "Premi E per attivare Power Jump!";
    }

    private void AttivaPowerUpJump()
    {
        if (player == null) return;

        powerJumpActive = true;
        hasPowerJump = false;
        powerUpText.text = "Power Jump attivo!";
        powerUpTimer = powerUpDuration;

        player.IniziaPowerJump(powerUpDuration);
    }

    private void DisattivaPowerUpJump()
    {
        powerJumpActive = false;
        powerUpText.text = "";
        player.FermaPowerJump();
    }

    internal void MostraPulsanteRespawn()
    {
        throw new NotImplementedException();
    }
}
