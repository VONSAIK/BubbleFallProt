using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;

public class Slime : MonoBehaviour
{
    [SerializeField] private SlimeColor _slimeColor;
    
    private EventBus _eventBus;

    public SlimeColor SlimeColor => _slimeColor;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }
}
