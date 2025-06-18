using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class ScoreController : IService
{
    private const int _priceForSlime = 10;

    private EventBus _eventBus;
    private int _score;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _score = 0;

        _eventBus.Subscribe<GridGroupPoppedSignal>(OnGroupPopped);
        _eventBus.Subscribe<GridSlimeFallingDownSignal>(OnFloatingSlimes);
    }

    private void OnGroupPopped(GridGroupPoppedSignal signal)
    {
        int points = signal.Group.Count * _priceForSlime;
        AddScore(points);
        Debug.Log($"+{points} ���� �� �����");
    }

    private void OnFloatingSlimes(GridSlimeFallingDownSignal signal)
    {
        int points = signal.Slimes.Count * _priceForSlime;
        AddScore(points);
        Debug.Log($" +{points} ���� �� ������ ������");
    }

    private void AddScore(int amount)
    {
        _score += amount;
        _eventBus.Invoke(new ScoreUpdatedSignal(_score));
    }
}
