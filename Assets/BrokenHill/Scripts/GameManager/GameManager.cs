using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Contador global")]
    [SerializeField] private int totalEnemies = 0;
    [SerializeField] private int defeatedEnemies = 0;

    [Header("UI de victoria")]
    [SerializeField] private GameObject victoryPanel;

    private bool gameEnded = false;

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

    public void EnemyDefeated()
    {
        if (gameEnded) return;

        defeatedEnemies++;
        Debug.Log("Enemigos eliminados: " + defeatedEnemies + "/" + totalEnemies);

        if (defeatedEnemies >= totalEnemies)
        {
            gameEnded = true;
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("¡Mataste a todos los enemigos del juego!");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public int GetDefeatedEnemies()
    {
        return defeatedEnemies;
    }

    public int GetTotalEnemies()
    {
        return totalEnemies;
    }
}
