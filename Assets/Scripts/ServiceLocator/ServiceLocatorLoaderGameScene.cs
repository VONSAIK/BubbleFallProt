using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using UnityEngine.PlayerLoop;

public class ServiceLocatorLoaderGameScene : MonoBehaviour
{
    [SerializeField] private PoolsController _poolsController;
    [SerializeField] private SlimeShooter _slimeShooter;
    [SerializeField] private SlimeHexGridSpawner _slimeHexGridSpawner;
    [SerializeField] private HexGridController _hexGridController;
    [SerializeField] private SlimeMover _slimeMover;

    private EventBus _eventBus;

    private void Awake()
    {
        _eventBus = new EventBus();

        RegisterService();
        Initialization();
    }

    private void RegisterService()
    {
        ServiceLocator.Init();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register<PoolsController>(_poolsController);
        ServiceLocator.Current.Register<SlimeShooter>(_slimeShooter);
        ServiceLocator.Current.Register<SlimeHexGridSpawner>(_slimeHexGridSpawner);
        ServiceLocator.Current.Register<HexGridController>(_hexGridController);
        ServiceLocator.Current.Register<SlimeMover>(_slimeMover);
    }

    private void Initialization()
    {
        _poolsController.Init();
        _slimeShooter.Init();
        _slimeMover.Init();
        _hexGridController.Init();
        _slimeHexGridSpawner.Init();
    }
}
