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
    [SerializeField] private GridGroupDetector _slimeGroupDetector;
    [SerializeField] private GridDestroyer _slimeDestroyer;
    [SerializeField] private GameController _gameController;

    private EventBus _eventBus;
    private ScoreController _scoreController;
    private GridDropDownController _gridDropDownController;

    private void Awake()
    {
        _eventBus = new EventBus();
        _scoreController = new ScoreController();
        _gridDropDownController = new GridDropDownController();

        RegisterService();
        Initialization();
    }

    private void RegisterService()
    {
        ServiceLocator.Init();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_scoreController);
        ServiceLocator.Current.Register<PoolsController>(_poolsController);
        ServiceLocator.Current.Register<SlimeShooter>(_slimeShooter);
        ServiceLocator.Current.Register<SlimeHexGridSpawner>(_slimeHexGridSpawner);
        ServiceLocator.Current.Register<HexGridController>(_hexGridController);
        ServiceLocator.Current.Register<SlimeMover>(_slimeMover);
        ServiceLocator.Current.Register<GridGroupDetector>(_slimeGroupDetector);
        ServiceLocator.Current.Register<GridDestroyer>(_slimeDestroyer);
        ServiceLocator.Current.Register(_gridDropDownController);
        ServiceLocator.Current.Register<GameController>(_gameController);
    }

    private void Initialization()
    {
        _poolsController.Init();
        _slimeShooter.Init();
        _slimeMover.Init();
        _hexGridController.Init();
        _slimeHexGridSpawner.Init();
        _gridDropDownController.Init();
        _slimeGroupDetector.Init();
        _slimeDestroyer.Init();
        _scoreController.Init();
        _gameController.Init();
    }
}
