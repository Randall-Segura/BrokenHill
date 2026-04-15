using TMPro;
using UnityEngine;

public class EnemyCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;

    private void Start()
    {
        UpdateCounter();
    }

    private void Update()
    {
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        if (GameManager.Instance != null && counterText != null)
        {
            counterText.text = GameManager.Instance.GetDefeatedEnemies()
                + " / "
                + GameManager.Instance.GetTotalEnemies()
                + " Enemigos Eliminados";
        }
    }
}
