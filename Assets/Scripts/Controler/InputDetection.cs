using System;
using Level;
using UnityEngine;

namespace Controller
{
    public class InputDetection : MonoBehaviour
    {
        [SerializeField] private GamePauser _gamePauser;
        [SerializeField] private AudioSource _audioSource;

        private float _minSwipeValue = 30;
        private bool _isSwiping;
        private bool _isMobile;
        private Vector3 _tapPosition;
        private Vector3 _swipeDelta;
        private Camera _camera;
        private Grille _currentGrille;

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
                    if (Input.GetMouseButtonDown(0))
                    {
                        TrySelectGrille(Input.mousePosition);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        UnselectGrille();
                        ResetSwipe();
                    }
                }
                else
                {
                    if (Input.touchCount > 0)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            TrySelectGrille(Input.GetTouch(0).position);
                        }
                        else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
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
            Ray ray = _camera.ScreenPointToRay(tapPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.TryGetComponent(out Grille grille))
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

            if (_swipeDelta.magnitude > _minSwipeValue)
            {
                Vector3 direction;

                if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                {
                    direction = _swipeDelta.x > 0 ? Vector3.right : Vector3.left;
                }
                else
                {
                    direction = _swipeDelta.y > 0 ? Vector3.up : Vector3.down;
                }

                Swiped?.Invoke(direction);
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