using System.Collections.Generic;
using UnityEngine;
using YandexSDK;

namespace UI
{
    public class LeaderboardView : GamePanel
    {
        [SerializeField] private LeaderboardPlayerView _template;
        [SerializeField] private LeaderboardDataChanger _leaderboardDataChanger;

        private List<LeaderboardPlayerView> _leaderboardPlayerViews = new List<LeaderboardPlayerView>();

        protected override void OnEnable()
        {
            _leaderboardDataChanger.Created += OnCreated;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            _leaderboardDataChanger.Created -= OnCreated;
            base.OnDisable();
        }

        private void Clear()
        {
            foreach (var playerView in _leaderboardPlayerViews)
            {
                Destroy(playerView.gameObject);
            }

            _leaderboardPlayerViews.Clear();
        }

        private void OnCreated(List<LeaderboardPlayer> leaderboardPlayers)
        {
            Clear();

            foreach (var player in leaderboardPlayers)
            {
                LeaderboardPlayerView leaderboardPlayerView = Instantiate(_template, transform);
                leaderboardPlayerView.Init(player.Number, player.Name, player.Level);
                _leaderboardPlayerViews.Add(leaderboardPlayerView);
            }
        }
    }
}