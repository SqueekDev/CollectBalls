using UnityEngine;
using UI;
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
        [SerializeField] private GamePanel _lostPanel;
        [SerializeField] private AudioSource _lostSound;
        [SerializeField] private float _startTime;

        private float _currentLevelTime;
        private float _time;
        private bool _isCounting;

        public float CurrentTime => _time;

        private void OnEnable()
        {
            _levelChanger.LevelChanged += OnLevelChanged;
            _levelChanger.LevelRestarted += OnLevelRestarted;
            _adShower.VideoAdShowed += OnVideoAdShowed;
        }

        private void OnDisable()
        {
            _levelChanger.LevelChanged -= OnLevelChanged;
            _levelChanger.LevelRestarted -= OnLevelRestarted;
            _adShower.VideoAdShowed -= OnVideoAdShowed;
        }

        private void Update()
        {
            if (_gamePauser.IsPaused == false && _isCounting)
            {
                _time -= Time.deltaTime;

                if (_time <= 0)
                {
                    _lostPanel.gameObject.SetActive(true);
                    _lostSound.Play();
                    _isCounting = false;
                }
            }
        }

        private void OnVideoAdShowed()
        {
            _time += RewardExtraTime;
            _isCounting = true;
            _lostPanel.gameObject.SetActive(false);
        }

        private void OnLevelChanged(int levelNumber)
        {
            _currentLevelTime = _startTime + (levelNumber / LevelDivider) * ExtraTimePerLevel;
            _time = _currentLevelTime;
            _isCounting = true;
        }

        private void OnLevelRestarted()
        {
            _time = _currentLevelTime;
            _isCounting = true;
        }
    }
}