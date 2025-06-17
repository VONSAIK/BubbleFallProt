using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class SlimeAttachHandler : MonoBehaviour, IService
{
    private EventBus _eventBus;
    private HexGridController _grid;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        
    }

    
}
