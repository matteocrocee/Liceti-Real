using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    public static CoinCollector Instance;

    public int coinCount = 0;
    public Text coinText; // Assegna nella UI (Canvas)

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoin()
    {
        coinCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coinCount;
    }
}