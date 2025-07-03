using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Coin = 0;
    public TextMeshProUGUI punteggioText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AggiungiPunti(int valore)
    {
        Coin += valore;
        AggiornaUI();
    }

    private void AggiornaUI()
    {
        if (punteggioText != null)
        {
            punteggioText.text = "Coin: " + Coin;
        }
    }
}
