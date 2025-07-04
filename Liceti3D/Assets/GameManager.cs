using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI puntiText;
    public TextMeshProUGUI speedInstantKillText;
    public TextMeshProUGUI powerJumpText;

    private int punti = 0;
    private Personaggio2 player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Personaggio2>();
        AggiornaPuntiUI();
        speedInstantKillText.gameObject.SetActive(false);
        powerJumpText.gameObject.SetActive(false);
    }

    public void AggiungiPunti(int valore)
    {
        punti += valore;
        AggiornaPuntiUI();
    }

    private void AggiornaPuntiUI()
    {
        if (puntiText != null)
            puntiText.text = "Punti: " + punti.ToString();
    }

    public void RaccogliSpeedInstantKill(float durata)
    {
        if (player != null)
        {
            player.IniziaSpeedInstantKill(durata);
            speedInstantKillText.gameObject.SetActive(true);
            CancelInvoke(nameof(DisattivaSpeedInstantKillText));
            Invoke(nameof(DisattivaSpeedInstantKillText), durata);
        }
    }

    private void DisattivaSpeedInstantKillText()
    {
        speedInstantKillText.gameObject.SetActive(false);
    }

    public void RaccogliPowerUpJump(float durata)
    {
        if (player != null)
        {
            player.IniziaPowerJump(durata);
            powerJumpText.gameObject.SetActive(true);
            CancelInvoke(nameof(DisattivaPowerJumpText));
            Invoke(nameof(DisattivaPowerJumpText), durata);
        }
    }

    private void DisattivaPowerJumpText()
    {
        powerJumpText.gameObject.SetActive(false);
    }
}
