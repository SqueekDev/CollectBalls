using UnityEngine;
using UI;

namespace Controller
{
    public class GamePauser : MonoBehaviour
    {
        [SerializeField] private LevelChanger _levelChanger;
        [SerializeField] private GamePanel _loginPanel;
        [SerializeField] private GamePanel _lostPanel;
        [SerializeField] private GamePanel _finishPanel;
        [SerializeField] private LeaderboardView _leaderboardPanel;

        public bool IsPaused { get; private set; }

        private void OnEnable()
        {
            _levelChanger.PanelOpened += OnLevelChangerPanelOpened;
            _loginPanel.Opened += OnPanelOpened;
            _loginPanel.Closed += OnPanelClosed;
            _lostPanel.Opened += OnPanelOpened;
            _lostPanel.Closed += OnPanelClosed;
            _finishPanel.Opened += OnPanelOpened;
            _finishPanel.Closed += OnPanelClosed;
            _leaderboardPanel.Opened += OnPanelOpened;
            _leaderboardPanel.Closed += OnPanelClosed;
        }

        private void OnDisable()
        {
            _levelChanger.PanelOpened -= OnLevelChangerPanelOpened;
            _loginPanel.Opened -= OnPanelOpened;
            _loginPanel.Closed -= OnPanelClosed;
            _lostPanel.Opened -= OnPanelOpened;
            _lostPanel.Closed -= OnPanelClosed;
            _finishPanel.Opened -= OnPanelOpened;
            _finishPanel.Closed -= OnPanelClosed;
            _leaderboardPanel.Opened -= OnPanelOpened;
            _leaderboardPanel.Closed -= OnPanelClosed;
        }

        private void OnPanelOpened()
        {
            IsPaused = true;
        }

        private void OnPanelClosed()
        {
            IsPaused = false;
        }

        private void OnLevelChangerPanelOpened(bool opened)
        {
            IsPaused = opened;
        }
    }
}