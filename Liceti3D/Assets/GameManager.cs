using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TMP_Text punteggioText;    // Testo punteggio (TextMeshPro)
    public TMP_Text powerUpText;      // Testo power-up (TextMeshPro)

    private int punteggio = 0;
    private bool hasSpeedPowerUp = false;
    private float powerUpDuration = 5f;
    private float powerUpTimer = 0f;

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

        if (powerUpTimer > 0f)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                powerUpTimer = 0f;
                // Il power-up termina: il personaggio si resetta da solo (via coroutine)
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

    private void AttivaPowerUpSpeed()
    {
        Debug.Log("Power-up Speed attivato!");
        hasSpeedPowerUp = false;
        powerUpText.text = "";
        powerUpTimer = powerUpDuration;

        Personaggio2 player = FindObjectOfType<Personaggio2>();
        if (player != null)
        {
            player.AttivaSpeedBoost(powerUpDuration);
        }
    }
}
