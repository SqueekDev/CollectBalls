using System;
using Level;
using UnityEngine;

namespace Controller
{
    public class InputDetection : MonoBehaviour
    {
        private const float MinSwipeValue = 30;

        [SerializeField] private GamePauseFlag _gamePauser;
        [SerializeField] private FenceSelector _fenceSelector;

        private bool _isSwiping;
        private bool _isMobile;
        private Vector3 _tapPosition;
        private Vector3 _swipeDelta;
        private Vector3 _swipeDirection;

        public static event Action<Vector3> Swiped;
        public event Action<Vector3> ButtonPressed;
        public event Action ButtonReleased;

        private void Awake()
        {
            _isMobile = Application.isMobilePlatform;
        }

        private void OnEnable()
        {
            _fenceSelector.Selected += OnFenceSelected;
        }

        private void Update()
        {
            if (_gamePauser.IsPaused)
            {
                return;
            }

            if (_isMobile == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ButtonPressed?.Invoke(Input.mousePosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    ButtonReleased?.Invoke();
                    ResetSwipe();
                }
            }
            else
            {
                if (Input.touchCount <= 0)
                {
                    return;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    ButtonPressed?.Invoke(Input.GetTouch(0).position);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled
                    || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ButtonReleased?.Invoke();
                    ResetSwipe();
                }
            }

            CheckSwipe();
        }

        private void OnDisable()
        {
            _fenceSelector.Selected -= OnFenceSelected;
        }

        private void CheckSwipe()
        {
            if (_isSwiping && Input.GetMouseButton(0))
            {
                _swipeDelta = Input.mousePosition - _tapPosition;
            }

            if (_swipeDelta.magnitude > MinSwipeValue)
            {
                if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                {
                    _swipeDirection = _swipeDelta.x > 0 ? Vector3.right : Vector3.left;
                }
                else
                {
                    _swipeDirection = _swipeDelta.y > 0 ? Vector3.up : Vector3.down;
                }

                Swiped?.Invoke(_swipeDirection);
                ResetSwipe();
            }
        }

        private void ResetSwipe()
        {
            _isSwiping = false;
            _swipeDelta = Vector3.zero;
        }

        private void OnFenceSelected(IronFenceSwiper fence, Vector3 tapPosition)
        {
            _isSwiping = true;
            _tapPosition = tapPosition;
        }
    }
}