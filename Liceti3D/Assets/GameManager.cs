using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TMP_Text punteggioText;
    public TMP_Text powerUpText;

    private int punteggio = 0;
    private bool hasSpeedInstantKill = false;
    private bool speedInstantKillActive = false;
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
        if (hasSpeedInstantKill && !speedInstantKillActive)
        {
            powerUpText.text = "Premi E per attivare Speed Instant Kill! Premi Q per usarlo.";
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

    public void RaccogliPowerUpSpeedInstantKill()
    {
        hasSpeedInstantKill = true;
        powerUpText.text = "Premi E per attivare Speed Instant Kill! Premi Q per usarlo.";
    }

    private void AttivaPowerUpSpeedInstantKill()
    {
        if (player == null) return;

        speedInstantKillActive = true;
        hasSpeedInstantKill = false;
        powerUpText.text = "Speed Instant Kill attivo! Premi Q per usarlo.";
        powerUpTimer = powerUpDuration;

        player.IniziaSpeedInstantKill(powerUpDuration);
    }

    private void DisattivaPowerUpSpeedInstantKill()
    {
        speedInstantKillActive = false;
        powerUpText.text = "";
        player.FermaSpeedInstantKill();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        powerUpText.text = "Game Over!";
        Time.timeScale = 0f;
    }
}
