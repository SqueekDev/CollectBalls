using System;
using Global;
using UnityEngine;
using YandexSDK;

namespace Controller
{
    public class Timer : MonoBehaviour
    {
        private const float RewardExtraTime = 20f;
        private const float ExtraTimePerLevel = 10f;
        private const int LevelDivider = 10;

        [SerializeField] private LevelChanger _levelChanger;
        [SerializeField] private GamePauser _gamePauser;
        [SerializeField] private AdShower _adShower;
        [SerializeField] private float _startTime;

        private float _currentLevelTime;
        private float _time;
        private bool _isCounting;

        public event Action TimeExpired;

        public float CurrentTime => _time;

        private void OnEnable()
        {
            _levelChanger.Changed += OnLevelChanged;
            _adShower.VideoAdShowed += OnVideoAdShowed;
        }

        private void OnDisable()
        {
            _levelChanger.Changed -= OnLevelChanged;
            _adShower.VideoAdShowed -= OnVideoAdShowed;
        }

        private void Update()
        {
            if (_gamePauser.IsPaused == false && _isCounting)
            {
                _time -= Time.deltaTime;

                if (_time <= GlobalValues.Zero)
                {
                    TimeExpired?.Invoke();
                    _isCounting = false;
                }
            }
        }

        private void OnVideoAdShowed()
        {
            _time += RewardExtraTime;
            _isCounting = true;
        }

        private void OnLevelChanged(int levelNumber)
        {
            _currentLevelTime = _startTime + ((levelNumber / LevelDivider) * ExtraTimePerLevel);
            _time = _currentLevelTime;
            _isCounting = true;
        }
    }
}