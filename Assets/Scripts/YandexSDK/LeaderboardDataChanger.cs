using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Agava.YandexGames;

public class LeaderboardDataChanger : MonoBehaviour
{
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private LevelChanger _levelChanger;
    [SerializeField] private LeaderboardButton _leaderboardButton;
    [SerializeField] private LoginAcceptButton _loginAcceptButton;

    private const string _leaderboardName = "Leaderboard";
    private const int _minPlayersCount = 1;
    private const int _maxPlayersCount = 5;

    private List<LeaderboardPlayer> _leaderboardPlayers = new List<LeaderboardPlayer>();

    public UnityAction<List<LeaderboardPlayer>> Created;

    private void OnEnable()
    {
        _levelChanger.LevelChanged += OnLevelChanged;
        _leaderboardButton.AutorizationCompleted += TryOpenPanel;
        _loginAcceptButton.ButtonClicked += TryOpenPanel;
    }

    private void OnDisable()
    {
        _levelChanger.LevelChanged -= OnLevelChanged;
        _leaderboardButton.AutorizationCompleted -= TryOpenPanel;
        _loginAcceptButton.ButtonClicked -= TryOpenPanel;
    }

    private void OnLevelChanged(int level)
    {
        if (PlayerAccount.IsAuthorized == false)
            return;

        Leaderboard.GetPlayerEntry(_leaderboardName, (result) =>
        {
            if (result == null || result.score < level)
                Leaderboard.SetScore(_leaderboardName, level);
        });
    }

    private void TryOpenPanel()
    {
        PlayerAccount.Authorize();

        if (PlayerAccount.IsAuthorized)
            PlayerAccount.RequestPersonalProfileDataPermission();

        if (PlayerAccount.IsAuthorized == false)
            return;

        _leaderboardPanel.SetActive(true);
        FillTable();
    }

    private void FillTable()
    {
        if (PlayerAccount.IsAuthorized == false)
            return;

        _leaderboardPlayers.Clear();
        Leaderboard.GetEntries(_leaderboardName, result =>
        {
            int results = result.entries.Length;
            results = Mathf.Clamp(results, _minPlayersCount, _maxPlayersCount);

            for (int i = 0; i < results; i++)
            {
                string number = (i + 1).ToString();
                string level = result.entries[i].score.ToString();
                string playerName = result.entries[i].player.publicName;

                if (string.IsNullOrEmpty(playerName))
                    playerName = "Anonymous";

                _leaderboardPlayers.Add(new LeaderboardPlayer(number, playerName, level));
            }

            Created?.Invoke(_leaderboardPlayers);
        });
    }
}
