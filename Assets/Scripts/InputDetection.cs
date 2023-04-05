using UnityEngine;
using UnityEngine.Events;

public class InputDetection : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;

    private float _minSwipeValue = 50;
    private bool _isSwiping;
    private Vector3 _tapPosition;
    private Vector3 _swipeDelta;
    private Camera _camera;
    private Grille _currentGrille;

    public static event UnityAction<Vector3> Swiped;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_levelController.IsPaused == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.TryGetComponent(out Grille grille))
                {
                    _currentGrille = grille;
                    _currentGrille.StartSwipe();
                    _isSwiping = true;
                    _tapPosition = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_currentGrille != null)
                {
                    _currentGrille.FinishSwipe();
                    _currentGrille = null;
                }

                ResetSwipe();
            }

            CheckSwipe();
        }        
    }

    private void CheckSwipe()
    {
        if (_isSwiping && Input.GetMouseButton(0))
            _swipeDelta = Input.mousePosition - _tapPosition;

        if (_swipeDelta.magnitude > _minSwipeValue)
        {
            Vector3 direction;

            if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                direction = _swipeDelta.x > 0 ? Vector3.right : Vector3.left;
            else
                direction = _swipeDelta.y > 0 ? Vector3.up : Vector3.down;

            Swiped?.Invoke(direction);
            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        _isSwiping = false;
        _swipeDelta = Vector3.zero;
    }
}
