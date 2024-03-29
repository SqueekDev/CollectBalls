using System;
using System.Collections.Generic;
using Agava.YandexGames;
using Controller;
using Lean.Localization;
using UI;
using UnityEngine;

namespace YandexSDK
{
    public class LeaderboardDataChanger : MonoBehaviour
    {
        private const string LeaderboardName = "Leaderboard";
        private const int MinPlayersCount = 1;
        private const int MaxPlayersCount = 5;

        [SerializeField] private GameObject _leaderboardPanel;
        [SerializeField] private LevelChanger _levelChanger;
        [SerializeField] private LeaderboardButton _leaderboardButton;
        [SerializeField] private LeanPhrase _anonymousText;
        [SerializeField] private GameButton _loginAcceptButton;

        private List<LeaderboardPlayer> _leaderboardPlayers = new List<LeaderboardPlayer>();

        public Action<List<LeaderboardPlayer>> Created;

        private void OnEnable()
        {
            _levelChanger.Changed += OnLevelChanged;
            _leaderboardButton.AutorizationCompleted += TryOpenPanel;
            _loginAcceptButton.Clicked += TryOpenPanel;
        }

        private void OnDisable()
        {
            _levelChanger.Changed -= OnLevelChanged;
            _leaderboardButton.AutorizationCompleted -= TryOpenPanel;
            _loginAcceptButton.Clicked -= TryOpenPanel;
        }

        private void TryOpenPanel()
        {
            PlayerAccount.Authorize();

            if (PlayerAccount.IsAuthorized)
            {
                PlayerAccount.RequestPersonalProfileDataPermission();
            }

            if (PlayerAccount.IsAuthorized == false)
            {
                return;
            }

            _leaderboardPanel.SetActive(true);
            FillTable();
        }

        private void FillTable()
        {
            if (PlayerAccount.IsAuthorized == false)
            {
                return;
            }

            _leaderboardPlayers.Clear();
            Leaderboard.GetEntries(LeaderboardName, result =>
            {
                int results = result.entries.Length;
                results = Mathf.Clamp(results, MinPlayersCount, MaxPlayersCount);

                for (int i = 0; i < results; i++)
                {
                    string number = (i + 1).ToString();
                    string level = result.entries[i].score.ToString();
                    string playerName = result.entries[i].player.publicName;

                    if (string.IsNullOrEmpty(playerName))
                    {
                        playerName = LeanLocalization.GetTranslationText(_anonymousText.name);
                    }

                    _leaderboardPlayers.Add(new LeaderboardPlayer(number, playerName, level));
                }

                Created?.Invoke(_leaderboardPlayers);
            });
        }

        private void OnLevelChanged(int level)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (PlayerAccount.IsAuthorized == false)
            {
                return;
            }

            Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
            {
                if (result == null || result.score < level)
                {
                    Leaderboard.SetScore(LeaderboardName, level);
                }
            });
#endif
        }
    }
}