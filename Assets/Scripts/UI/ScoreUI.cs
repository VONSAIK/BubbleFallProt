using TMPro;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ScoreUpdatedSignal>(OnScoreUpdated);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
            _eventBus.Unsubscribe<ScoreUpdatedSignal>();
    }

    private void OnScoreUpdated(ScoreUpdatedSignal signal)
    {
        scoreText.text = $"{signal.Score}";
    }
}
