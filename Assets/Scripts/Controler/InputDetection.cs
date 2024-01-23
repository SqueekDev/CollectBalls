using System;
using Global;
using Level;
using UnityEngine;

namespace Controller
{
    public class InputDetection : MonoBehaviour
    {
        private const float MinSwipeValue = 30;

        [SerializeField] private GamePauser _gamePauser;
        [SerializeField] private AudioSource _audioSource;

        private bool _isSwiping;
        private bool _isMobile;
        private Vector3 _tapPosition;
        private Vector3 _swipeDelta;
        private Vector3 _swipeDirection;
        private Camera _camera;
        private GrilleSwiper _currentGrille;
        private RaycastHit _hit;
        private Ray _ray;

        public static event Action<Vector3> Swiped;

        private void Awake()
        {
            _camera = Camera.main;
            _isMobile = Application.isMobilePlatform;
        }

        private void Update()
        {
            if (_gamePauser.IsPaused == false)
            {
                if (_isMobile == false)
                {
                    if (Input.GetMouseButtonDown(GlobalValues.Zero))
                    {
                        TrySelectGrille(Input.mousePosition);
                    }
                    else if (Input.GetMouseButtonUp(GlobalValues.Zero))
                    {
                        UnselectGrille();
                        ResetSwipe();
                    }
                }
                else
                {
                    if (Input.touchCount > GlobalValues.Zero)
                    {
                        if (Input.GetTouch(GlobalValues.Zero).phase == TouchPhase.Began)
                        {
                            TrySelectGrille(Input.GetTouch(GlobalValues.Zero).position);
                        }
                        else if (Input.GetTouch(GlobalValues.Zero).phase == TouchPhase.Canceled
                            || Input.GetTouch(GlobalValues.Zero).phase == TouchPhase.Ended)
                        {
                            UnselectGrille();
                            ResetSwipe();
                        }
                    }
                }

                CheckSwipe();
            }
        }

        private void TrySelectGrille(Vector3 tapPosition)
        {
            _ray = _camera.ScreenPointToRay(tapPosition);

            if (Physics.Raycast(_ray, out _hit)
                && _hit.collider.gameObject.TryGetComponent(out GrilleSwiper grille))
            {
                _currentGrille = grille;
                _currentGrille.StartSwipe();
                _isSwiping = true;
                _currentGrille.Sounded += OnSounded;
                _tapPosition = tapPosition;
            }

        }

        private void UnselectGrille()
        {
            if (_currentGrille != null)
            {
                _currentGrille.Sounded -= OnSounded;
                _currentGrille.FinishSwipe();
                _currentGrille = null;
            }
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
                    _swipeDirection = _swipeDelta.x > GlobalValues.Zero ? Vector3.right : Vector3.left;
                }
                else
                {
                    _swipeDirection = _swipeDelta.y > GlobalValues.Zero ? Vector3.up : Vector3.down;
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

        private void OnSounded(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
}