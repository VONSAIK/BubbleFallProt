using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class SlimeMover : MonoBehaviour, IService
{
    [SerializeField] private float shootSpeed = 10f;

    private List<MovingSlimeData> _flyingSlimes = new();
    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscride<SlimeLaunchedSignal>(OnSlimeLaunched);
        _eventBus.Subscride<SlimeLandedSignal>(OnSlimeLanded);
        _eventBus.Subscride<SlimeAttachedSignal>(OnSlimeAttached);
    }

    private void OnSlimeLaunched(SlimeLaunchedSignal signal)
    {
        if (!_flyingSlimes.Exists(s => s.Slime == signal.Slime))
        {
            _flyingSlimes.Add(new MovingSlimeData(signal.Slime, signal.Direction));
        }
    }

    private void OnSlimeLanded(SlimeLandedSignal signal)
    {
        for (int i = 0; i < _flyingSlimes.Count; i++)
        {
            if (_flyingSlimes[i].Slime == signal.Slime)
            {
                var data = _flyingSlimes[i];
                data.IsFrozen = true;
                _flyingSlimes[i] = data;
                break;
            }
        }
    }

    private void OnSlimeAttached(SlimeAttachedSignal signal)
    {
        _flyingSlimes.RemoveAll(s => s.Slime == signal.Slime);
    }

    private void FixedUpdate()
    {
        float moveStep = shootSpeed * Time.fixedDeltaTime;
        float checkDistance = moveStep * 1.5f;
        float radius = 0.25f;

        for (int i = 0; i < _flyingSlimes.Count; i++)
        {
            var data = _flyingSlimes[i];

            if (data.Slime == null || data.IsFrozen)
                continue;

            Slime slime = data.Slime;
            Vector3 direction = data.Direction;
            Vector3 origin = slime.transform.position;

            if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, checkDistance, data.StopLayers))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    direction = new Vector3(-direction.x, 0f, direction.z).normalized;
                    slime.transform.position = hit.point + direction * 0.01f;

                    data.Direction = direction;
                    _flyingSlimes[i] = data;
                    continue;
                }

                if (hit.collider.CompareTag("Slime"))
                {
                    slime.transform.position = hit.point - direction * 0.01f;
                    _eventBus.Invoke(new SlimeLandedSignal(slime));
                    continue;
                }
            }

            slime.transform.position += direction * moveStep;
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SlimeLaunchedSignal>();
        _eventBus.Unsubscribe<SlimeLandedSignal>();
        _eventBus.Unsubscribe<SlimeAttachedSignal>();
    }
}
