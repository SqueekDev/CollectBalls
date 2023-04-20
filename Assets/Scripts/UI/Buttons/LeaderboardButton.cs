using UnityEngine;
using UnityEngine.Events;
using Agava.YandexGames;

public class LeaderboardButton : GameButton
{
    [SerializeField] private GamePanel _loginPanel;
    [SerializeField] private LevelController _levelController;

    public event UnityAction AutorizationCompleted;

    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        if (_levelController.IsPaused == false)
        {
            if (PlayerAccount.IsAuthorized == false)
                _loginPanel.gameObject.SetActive(true);
            else
                AutorizationCompleted?.Invoke();
        }
    }
}
