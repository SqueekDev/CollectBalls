using UnityEngine;

namespace UI
{
    public class LoginPanel : GamePanel
    {
        [SerializeField] private LeaderboardView _leaderboardView;

        protected override void OnEnable()
        {
            _leaderboardView.Opened += OnLeaderboardPanelOpened;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            _leaderboardView.Opened -= OnLeaderboardPanelOpened;
            base.OnDisable();
        }

        private void OnLeaderboardPanelOpened()
        {
            gameObject.SetActive(false);
        }
    }
}