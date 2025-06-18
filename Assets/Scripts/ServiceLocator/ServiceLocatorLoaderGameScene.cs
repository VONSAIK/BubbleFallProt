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
    [SerializeField] private SlimeGroupDetector _slimeGroupDetector;
    [SerializeField] private SlimeDestroyer _slimeDestroyer;
    [SerializeField] private SlimeDropDownController _slimeDropDownController;

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
        ServiceLocator.Current.Register<SlimeGroupDetector>(_slimeGroupDetector);
        ServiceLocator.Current.Register<SlimeDestroyer>(_slimeDestroyer);
        ServiceLocator.Current.Register<SlimeDropDownController>(_slimeDropDownController);
    }

    private void Initialization()
    {
        _poolsController.Init();
        _slimeShooter.Init();
        _slimeMover.Init();
        _hexGridController.Init();
        _slimeHexGridSpawner.Init();
        _slimeDropDownController.Init();
        _slimeGroupDetector.Init();
        _slimeDestroyer.Init();
    }
}
