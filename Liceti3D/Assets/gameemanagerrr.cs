using UnityEngine;

public class gameemanagerrr : MonoBehaviour
{
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);  // Nascondi il pannello all’inizio
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Ferma il gioco (optional)
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Riattiva il gioco
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
