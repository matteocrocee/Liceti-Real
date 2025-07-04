using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public TMP_Text punteggioText;

    private int punteggio = 0;

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
}
