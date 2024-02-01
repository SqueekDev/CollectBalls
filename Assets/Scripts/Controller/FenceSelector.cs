using System;
using Level;
using UnityEngine;

namespace Controller
{
    public class FenceSelector : MonoBehaviour
    {
        [SerializeField] private InputDetection _inputDetection;
        
        private Ray _ray;
        private Camera _camera;
        private RaycastHit _hit;
        private IronFenceSwiper _currentFence;

        public event Action<IronFenceSwiper, Vector3> Selected;
        public event Action<IronFenceSwiper> Unselected;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            _inputDetection.ButtonPressed += OnButtonPressed;
            _inputDetection.ButtonReleased += OnButtonReleased;
        }

        private void OnDisable()
        {
            _inputDetection.ButtonPressed -= OnButtonPressed;
            _inputDetection.ButtonReleased -= OnButtonReleased;
        }

        private void OnButtonPressed(Vector3 tapPosition)
        {
            _ray = _camera.ScreenPointToRay(tapPosition);

            if (Physics.Raycast(_ray, out _hit)
                && _hit.collider.gameObject.TryGetComponent(out IronFenceSwiper grille))
            {
                _currentFence = grille;
                _currentFence.StartSwipe();
                Selected?.Invoke(_currentFence, tapPosition);
            }
        }

        private void OnButtonReleased()
        {
            if (_currentFence != null)
            {
                _currentFence.FinishSwipe();
                Unselected?.Invoke(_currentFence);
                _currentFence = null;
            }
        }
    }
}