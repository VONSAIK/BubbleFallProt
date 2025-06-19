using UnityEngine;
using UnityEngine.SceneManagement;
using CustomEventBus;
using CustomEventBus.Signals;

public class GameController : MonoBehaviour, IService
{
    [SerializeField] private GameObject gameOverScreen;

    private EventBus _eventBus;
    private bool _isGameOver = false;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<GameOverSignal>(OnGameOver);

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    private void OnGameOver(GameOverSignal signal)
    {
        if (_isGameOver) return;

        _isGameOver = true;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        Debug.Log("Game Over!");
    }

    public void OnRestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
