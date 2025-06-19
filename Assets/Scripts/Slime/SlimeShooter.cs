using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class SlimeShooter : MonoBehaviour, IService
{
    [SerializeField] private Transform shootPoint;

    private Camera _mainCamera;
    private PoolsController _poolsController;
    private EventBus _eventBus;

    private Slime _currentSlime;
    private bool _canShoot = true;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _poolsController = ServiceLocator.Current.Get<PoolsController>();
        _mainCamera = Camera.main;

        _eventBus.Subscribe<SlimeAttachedSignal>(OnSlimeAttached);
        _eventBus.Subscribe<GameOverSignal>(OnGameOver);

        _canShoot = true;
        LoadNextSlime();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0) && _canShoot && _currentSlime != null)
        {
            Vector3 direction = GetShootDirection(Input.mousePosition);
            Shoot(direction);
        }
#else
        if (Input.touchCount > 0 && _canShoot && _currentSlime != null)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Vector3 direction = GetShootDirection(touch.position);
                Shoot(direction);
            }
        }
#endif
    }

    private void Shoot(Vector3 direction)
    {
        _eventBus.Invoke(new SlimeLaunchedSignal(_currentSlime, direction));
        _canShoot = false;
        _currentSlime = null;
    }

    private void LoadNextSlime()
    {
        _currentSlime = _poolsController.GetSlime();
        _currentSlime.transform.position = shootPoint.position;
        _currentSlime.transform.rotation = Quaternion.identity;
    }

    private void OnSlimeAttached(SlimeAttachedSignal signal)
    {
        _canShoot = true;
        LoadNextSlime();
    }

    private void OnGameOver(GameOverSignal signal)
    {
        _canShoot = false;
    }

    private Vector3 GetShootDirection(Vector3 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        Plane xzPlane = new Plane(Vector3.up, shootPoint.position);

        if (xzPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 direction = hitPoint - shootPoint.position;
            direction.y = 0f;
            return direction.normalized;
        }

        return Vector3.forward;
    }
}
