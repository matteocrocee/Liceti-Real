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
    private bool isSpeedBoostActive = false;
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
        if (hasSpeedPowerUp && !isSpeedBoostActive)
        {
            powerUpText.text = "Premi E per attivare Speed!";
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttivaPowerUpSpeed();
            }
        }

        if (isSpeedBoostActive)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                powerUpTimer = 0f;
                DisattivaPowerUpSpeed();
            }
        }

        if (player != null && !isSpeedBoostActive)
        {
            player.CorsaNormale(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
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
        powerUpText.text = "Premi E per attivare Speed!";
    }

    private void AttivaPowerUpSpeed()
    {
        if (player == null) return;

        isSpeedBoostActive = true;
        hasSpeedPowerUp = false;
        powerUpText.text = "Speed attivo!";
        powerUpTimer = powerUpDuration;

        player.AttivaSpeedBoost(powerUpDuration);
    }

    private void DisattivaPowerUpSpeed()
    {
        isSpeedBoostActive = false;
        powerUpText.text = "";
        player.DisattivaSpeedBoost();
    }
}
