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

        private void OnEnable()
        {
            _timer.TimeExpired += OnTimeExpired;
            _levelChanger.LevelFinished += OnLevelFinished;
        }

        private void Start()
        {
            if (_levelChanger.CurrentLevelNumber == TutorialLevelNumber)
            {
                _tutorial.gameObject.SetActive(true);
            }
            else
            {
                _tutorial.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _timer.TimeExpired -= OnTimeExpired;            
            _levelChanger.LevelFinished -= OnLevelFinished;
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