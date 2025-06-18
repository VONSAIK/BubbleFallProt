using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;

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

        var floating = _matrix.GetFloatingSlimes();
        List<Slime> detached = new();

        foreach (var hex in floating)
        {
            var slime = _matrix.GetSlime(hex);
            if (slime != null)
            {
                detached.Add(slime);
                _eventBus.Invoke(new DisposeSlimeSignal(slime));
                _matrix.Unregister(hex);
            }
        }

        if (detached.Count > 0)
        {
            Debug.Log($"Осипалось {detached.Count} слаймів.");
            _eventBus.Invoke(new SlimesDetachedSignal(detached));
        }

        _eventBus.Invoke(new SlimeStepDownSignal());
    }
    private IEnumerator InvokeStepDownNextFrame()
    {
        yield return null; 
        
    }


}

