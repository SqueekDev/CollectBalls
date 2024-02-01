using System;
using Controller;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
    public class IronFenceSwiper : MonoBehaviour
    {
        private const float Step = 0.2f;
        private const float Range = 1f;

        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioClip _swipedClip;
        [SerializeField] private AudioClip _blockedClip;

        private float _moveProgress;
        private bool _targetReached;
        private Vector3 _startPosition;
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;
        private Rigidbody _rigidbody;
        private MeshRenderer _meshRenderer;

        public event Action<AudioClip> Sounded;

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
            _moveProgress += Step;
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
            _targetReached = transform.localPosition == _targetPosition;

            if (_targetReached)
            {
                _targetPosition = _startPosition + direction * Range;
                RaycastHit hit;

                if (_rigidbody != null
                    && _rigidbody.SweepTest(direction, out hit, Range)
                    && (hit.collider.gameObject.TryGetComponent(out IronFenceSwiper grille)
                    || hit.collider.gameObject.TryGetComponent(out Obstacle obstacle)))
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
}