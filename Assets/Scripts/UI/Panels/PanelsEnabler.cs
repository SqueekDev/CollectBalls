using Controller;
using Global;
using UnityEngine;

namespace UI
{
    public class PanelsEnabler : MonoBehaviour
    {
        private const int TutorialLevelNumber = 1;

        [SerializeField] private Timer _timer;
        [SerializeField] private LevelChanger _levelChanger;
        [SerializeField] private LostPanel _lostPanel;
        [SerializeField] private FinishPanel _finishPanel;
        [SerializeField] private Tutorial _tutorial;

        private int _savedLevelNumber;
        private bool _isTutorialLevel;

        private void OnEnable()
        {
            _timer.TimeExpired += OnTimeExpired;
            _levelChanger.Finished += OnLevelFinished;
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey(GlobalValues.PlayerPrefsLevelKey))
            {
                _savedLevelNumber = PlayerPrefs.GetInt(GlobalValues.PlayerPrefsLevelKey);
                _isTutorialLevel = _savedLevelNumber == TutorialLevelNumber;
                _tutorial.gameObject.SetActive(_isTutorialLevel);
            }
            else
            {
                _tutorial.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            _timer.TimeExpired -= OnTimeExpired;            
            _levelChanger.Finished -= OnLevelFinished;
        }

        private void Enable(GamePanel gamePanel)
        {
            gamePanel.gameObject.SetActive(true);
        }

        private void OnTimeExpired()
        {
            Enable(_lostPanel);
        }

        private void OnLevelFinished()
        {
            Enable(_finishPanel);
        }
    }
}