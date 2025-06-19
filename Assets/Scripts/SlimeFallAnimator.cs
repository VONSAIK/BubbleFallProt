using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class SlimeFallAnimator : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 7f;
    [SerializeField] private float targetZ = -6f;

    private bool _isFalling = false;
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    public void StartFall()
    {
        _isFalling = true;

        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("FallingSlime");

        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        if (!_isFalling) return;

        transform.position += Vector3.back * fallSpeed * Time.deltaTime;

        if (transform.position.z <= targetZ)
        {
            _isFalling = false;
            _eventBus.Invoke(new DisposeSlimeSignal(GetComponent<Slime>()));
        }
    }
}
