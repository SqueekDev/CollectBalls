using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;
    [SerializeField] private AdShower _adShower;
    [SerializeField] private float _startTime;

    private const float _rewardExtraTime = 20f; 
    private float _currentLevelTime;
    private float _time;
    private bool _isCounting;

    public float CurrentTime => _time;

    public event UnityAction Expired;
    public event UnityAction Added;

    private void OnEnable()
    {
        _levelController.LevelChanged += OnLevelChanged;
        _levelController.LevelRestarted += OnLevelRestarted;
        _adShower.VideoAdShowed += OnVideoAdShowed;
    }

    private void OnDisable()
    {
        _levelController.LevelChanged -= OnLevelChanged;        
        _levelController.LevelRestarted -= OnLevelRestarted;
        _adShower.VideoAdShowed -= OnVideoAdShowed;
    }

    private void Update()
    {
        if (_levelController.IsPaused == false && _isCounting)
        {
            _time -= Time.deltaTime;

            if (_time <= 0)
            {
                Expired?.Invoke();
                _isCounting = false;
            }
        }
    }

    private void OnVideoAdShowed()
    {
        _time += _rewardExtraTime;
        _isCounting = true;
        Added?.Invoke();
    }

    private void OnLevelChanged(int levelNumber)
    {
        float extraTime = 10f;
        int divider = 10;
        _currentLevelTime = _startTime + (levelNumber / divider) * extraTime;
        _time = _currentLevelTime;
        _isCounting = true;
    }

    private void OnLevelRestarted()
    {
        _time = _currentLevelTime;
        _isCounting = true;
    }
}
