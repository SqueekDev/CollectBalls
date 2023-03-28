using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grille : MonoBehaviour
{
    [SerializeField] private float _step;
    [SerializeField] private float _range;

    private float _moveProgress;
    private bool _targetReached;
    private Vector3 _startPosition;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _targetReached = true;
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _currentPosition = _startPosition;
        _targetPosition = _startPosition;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(_currentPosition, _targetPosition, _moveProgress);
        _moveProgress += _step;

        if (transform.position == _targetPosition)
            _targetReached = true;
        else
            _targetReached = false;
    }

    public void StartSwipe()
    {
        InputDetection.Swiped += OnSwiped;
        _currentPosition = transform.position;
    }

    public void FinishSwipe()
    {
        InputDetection.Swiped -= OnSwiped;
    }

    private void OnSwiped(Vector3 direction)
    {
        if (_targetReached)
        {
            _targetPosition = _startPosition + direction * _range;
            RaycastHit hit;

            if (_rigidbody.SweepTest(direction, out hit, _range) && (hit.collider.gameObject.TryGetComponent(out Grille grille) || hit.collider.gameObject.TryGetComponent(out Obstacle obstacle)))
                _targetPosition = _startPosition;
            else
                _startPosition = _targetPosition;

            _moveProgress = 0;
        }
    }
}
