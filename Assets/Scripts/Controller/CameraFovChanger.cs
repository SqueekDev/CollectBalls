using UnityEngine;

namespace Controller
{
    public class CameraFovChanger : MonoBehaviour
    {
        private const int PortraitFov = 50;
        private const int LandscapeFov = 35;

        [SerializeField] private FieldsChanger _fieldChanger;

        private Camera _camera;
        private bool _isMobile;

        private void Awake()
        {
            _camera = Camera.main;
            _isMobile = Application.isMobilePlatform;
        }

        private void OnEnable()
        {
            _fieldChanger.FieldSizeChanged += OnFieldSizeChanged;
        }

        private void OnDisable()
        {
            _fieldChanger.FieldSizeChanged -= OnFieldSizeChanged;
        }

        private void OnFieldSizeChanged(int fieldSizeModifier)
        {
            if (_isMobile == false)
            {
                _camera.fieldOfView = LandscapeFov + fieldSizeModifier;
            }
            else
            {
                if (Screen.orientation == ScreenOrientation.Portrait)
                {
                    _camera.fieldOfView = PortraitFov;
                }
                else
                {
                    _camera.fieldOfView = LandscapeFov + fieldSizeModifier;
                }
            }
        }
    }
}