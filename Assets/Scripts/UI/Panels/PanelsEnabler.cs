using Controller;
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
        [SerializeField] private DataSaver _dataSaver;

        private int _savedLevelNumber;
        private bool _isTutorialLevel;

        private void OnEnable()
        {
            _timer.TimeExpired += OnTimeExpired;
            _levelChanger.Finished += OnLevelFinished;
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey(_dataSaver.KeyName))
            {
                _savedLevelNumber = PlayerPrefs.GetInt(_dataSaver.KeyName);
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