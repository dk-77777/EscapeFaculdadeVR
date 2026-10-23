using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Cartoes")]
    [SerializeField] private int totalCardsRequired = 3;
    [SerializeField] private int collectedCards = 0;

    [Header("Eventos")]
    public UnityEvent onCardCollected;
    public UnityEvent onAllCardsCollected;
    public UnityEvent onGameOver;
    public UnityEvent onVictory;

    public int CollectedCards => collectedCards;
    public int TotalCardsRequired => totalCardsRequired;
    public bool HasAllCards => collectedCards >= totalCardsRequired;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CollectCard()
    {
        if (collectedCards >= totalCardsRequired)
            return;

        collectedCards++;
        onCardCollected?.Invoke();

        if (HasAllCards)
            onAllCardsCollected?.Invoke();
    }

    public void GameOver()
    {
        onGameOver?.Invoke();
    }

    public void Victory()
    {
        onVictory?.Invoke();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
