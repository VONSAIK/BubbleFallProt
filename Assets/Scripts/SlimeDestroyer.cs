using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class SlimeDestroyer : MonoBehaviour, IService
{
    private HexGridMatrix _matrix;
    private PoolsController _pools;
    private EventBus _eventBus;

    public void Init()
    {
        _matrix = ServiceLocator.Current.Get<HexGridController>().GetMatrix();
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscride<SlimeGroupPoppedSignal>(OnGroupPopped);
    }

    private void OnGroupPopped(SlimeGroupPoppedSignal signal)
    {
        foreach (var hex in signal.Group)
        {
            Slime slime = _matrix.GetSlime(hex);
            if (slime != null)
            {
                _eventBus.Invoke(new DisposeSlimeSignal(slime));
                _matrix.Unregister(hex);
            }
        }

        Debug.Log($"Видалено групу з {signal.Group.Count} слаймів.");
    }



}

