using UnityEngine;

public class SlimeMoveAnimator : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.2f;

    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private float _elapsedTime = 0f;
    private Vector3 _startPosition;

    public void StartMoveTo(Vector3 target)
    {
        _startPosition = transform.position;
        _targetPosition = target;
        _elapsedTime = 0f;
        _isMoving = true;
    }

    private void Update()
    {
        if (!_isMoving) return;

        _elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsedTime / moveDuration);
        transform.position = Vector3.Lerp(_startPosition, _targetPosition, t);

        if (t >= 1f)
        {
            _isMoving = false;
        }
    }
}
