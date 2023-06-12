using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Agava.YandexGames;

public class LeaderboardDataChanger : MonoBehaviour
{
    private const string LeaderboardName = "Leaderboard";
    private const int MinPlayersCount = 1;
    private const int MaxPlayersCount = 5;

    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private LevelChanger _levelChanger;
    [SerializeField] private LeaderboardButton _leaderboardButton;
    [SerializeField] private LoginAcceptButton _loginAcceptButton;


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

        Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
        {
            if (result == null || result.score < level)
                Leaderboard.SetScore(LeaderboardName, level);
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
                    playerName = "Anonymous";

                _leaderboardPlayers.Add(new LeaderboardPlayer(number, playerName, level));
            }

            Created?.Invoke(_leaderboardPlayers);
        });
    }
}
