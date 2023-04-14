using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class Grille : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioClip _swipedClip;
    [SerializeField] private AudioClip _blockedClip;

    private float _step = 0.2f;
    private float _range = 1f;
    private float _moveProgress;
    private bool _targetReached;
    private Vector3 _startPosition;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;

    public event UnityAction<AudioClip> Sounded;

    private void Awake()
    {
        _targetReached = true;
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _startPosition = transform.localPosition;
        _currentPosition = _startPosition;
        _targetPosition = _startPosition;
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(_currentPosition, _targetPosition, _moveProgress);
        _moveProgress += _step;

        if (transform.position == _targetPosition)
            _targetReached = true;
        else
            _targetReached = false;
    }

    public void StartSwipe()
    {
        InputDetection.Swiped += OnSwiped;
        _particleSystem.Play();
        _currentPosition = transform.localPosition;
    }

    public void FinishSwipe()
    {
        InputDetection.Swiped -= OnSwiped;
        _particleSystem.Stop();
    }

    public void SetMaterial(Material material)
    {
        if (material != null)
        {
            _meshRenderer.material = material;
            _particleSystem.startColor = _meshRenderer.material.color;
        }
    }

    private void OnSwiped(Vector3 direction)
    {
        if (_targetReached)
        {
            _targetPosition = _startPosition + direction * _range;
            RaycastHit hit;

            if (_rigidbody != null && _rigidbody.SweepTest(direction, out hit, _range) && (hit.collider.gameObject.TryGetComponent(out Grille grille) || hit.collider.gameObject.TryGetComponent(out Obstacle obstacle)))
            {
                _targetPosition = _startPosition;
                Sounded?.Invoke(_blockedClip);
            }
            else
            {
                _startPosition = _targetPosition;
                Sounded?.Invoke(_swipedClip);
            }

            _moveProgress = 0;
        }
    }
}
