using UnityEngine;
using UnityEngine.UI;  // o TMPro se usi TextMeshPro
using TMPro;

public class PowerUpUI : MonoBehaviour
{
    public Text powerUpText; // trascina qui il testo nel canvas

    void Start()
    {
        powerUpText.text = "Power-Up: NESSUNO";
    }

    public void ShowPowerUpReady(string powerUpName)
    {
        powerUpText.text = $"Power-Up pronto: {powerUpName} (Premi E)";
    }

    public void ClearPowerUp()
    {
        powerUpText.text = "Power-Up: NESSUNO";
    }
}
