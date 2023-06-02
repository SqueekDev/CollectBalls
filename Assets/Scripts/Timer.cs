using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private const float _rewardExtraTime = 20f;
    
    [SerializeField] private LevelController _levelController;
    [SerializeField] private AdShower _adShower;
    [SerializeField] private float _startTime;

    private float _currentLevelTime;
    private float _time;
    private bool _isCounting;
    private float _extraTimePerLevel = 10f;
    private int _levelDivider = 10;

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
        _currentLevelTime = _startTime + (levelNumber / _levelDivider) * _extraTimePerLevel;
        _time = _currentLevelTime;
        _isCounting = true;
    }

    private void OnLevelRestarted()
    {
        _time = _currentLevelTime;
        _isCounting = true;
    }
}
