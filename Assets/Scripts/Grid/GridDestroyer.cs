using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;

public class GridDestroyer : MonoBehaviour, IService
{
    private HexGridMatrix _matrix;
    private PoolsController _pools;
    private EventBus _eventBus;

    public void Init()
    {
        _matrix = ServiceLocator.Current.Get<HexGridController>().GetMatrix();
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<GridGroupPoppedSignal>(OnGroupPopped);
    }

    private void OnGroupPopped(GridGroupPoppedSignal signal)
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

        var floating = _matrix.GetFloatingSlimes();
        List<Slime> detached = new();

        foreach (var hex in floating)
        {
            var slime = _matrix.GetSlime(hex);
            if (slime != null)
            {
                var animator = slime.GetComponent<SlimeFallAnimator>();
                if (animator != null)
                    animator.StartFall();

                _matrix.Unregister(hex); 
            }
        }

        if (detached.Count > 0)
        {
            _eventBus.Invoke(new GridSlimeFallingDownSignal(detached));
        }

        _eventBus.Invoke(new GridStepDownSignal());
    }
    private IEnumerator InvokeStepDownNextFrame()
    {
        yield return null; 
        
    }


}

