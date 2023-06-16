using UnityEngine;

public class CameraFovChanger : MonoBehaviour
{
    private const int PortraitFov = 50;
    private const int LandscapeFov = 35;

    [SerializeField] private LevelChanger _levelChanger;

    private Camera _camera;
    private bool _isMobile;
    private int _fieldSizeFovModifier;

    private void Awake()
    {
        _camera = Camera.main;
        _isMobile = Application.isMobilePlatform;
    }

    private void OnEnable()
    {
        _levelChanger.FieldSizeChanged += OnFieldSizeChanged;
    }

    private void OnDisable()
    {
        _levelChanger.FieldSizeChanged -= OnFieldSizeChanged;
    }

    private void Update()
    {
        if (_isMobile == false)
            _camera.fieldOfView = LandscapeFov + _fieldSizeFovModifier;
        else
            if (Screen.orientation == ScreenOrientation.Portrait)
                _camera.fieldOfView = PortraitFov;
            else
                _camera.fieldOfView = LandscapeFov + _fieldSizeFovModifier;
    }

    private void OnFieldSizeChanged(int fieldSizeModifier)
    {
        _fieldSizeFovModifier = fieldSizeModifier;
    }
}
