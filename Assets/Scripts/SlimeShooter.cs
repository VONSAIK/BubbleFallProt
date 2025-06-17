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
    private bool _canShoot;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _poolsController = ServiceLocator.Current.Get<PoolsController>();
        _mainCamera = Camera.main;

        _eventBus.Subscride<SlimeAttachedSignal>(OnSlimeAttached);

        _canShoot = true;
        LoadNextSlime();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _canShoot && _currentSlime != null)
        {
            Vector3 direction = GetShootDirection();
            _eventBus.Invoke(new SlimeLaunchedSignal(_currentSlime, direction));

            _canShoot = false;
            _currentSlime = null;
        }
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

    private Vector3 GetShootDirection()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
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
