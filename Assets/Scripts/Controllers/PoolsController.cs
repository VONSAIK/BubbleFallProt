using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsController : MonoBehaviour, IService
{
    [SerializeField] private List<Slime> _slimes;
    [SerializeField] private int _poolSize = 30;

    private EventBus _eventBus;

    private Dictionary<SlimeColor, ObjectPool<Slime>> _pools = new Dictionary<SlimeColor, ObjectPool<Slime>>();

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<DisposeSlimeSignal>(DisposeSlime);
    }

    public Slime GetSlime()
    {
        var slime = _slimes[Random.Range(0, _slimes.Count)];
        var pool = GetPool(slime);

        var item = pool.Get();

        return item;
    }

    private ObjectPool<Slime> GetPool(Slime slime)
    {
        SlimeColor slimeColor = slime.SlimeColor;
        ObjectPool<Slime> pool;

        if (!_pools.ContainsKey(slimeColor))
        {
            pool = new ObjectPool<Slime>(slime, _poolSize);
            _pools.Add(slimeColor, pool);
        }
        else
        {
            pool = _pools[slimeColor];
        }

        return pool;
    }

    private void DisposeSlime(DisposeSlimeSignal signal)
    {
        var slime =signal.Slime;
        var pool = GetPool(slime);

        pool.Realease(slime);

    }

}
