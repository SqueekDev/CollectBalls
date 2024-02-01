using System;
using Agava.YandexGames;
using Controller;
using UnityEngine;

namespace UI
{
    public class LeaderboardButton : GameButton
    {
        [SerializeField] private GamePanel _loginPanel;
        [SerializeField] private GamePauseFlag _gamePauser;

        public event Action AutorizationCompleted;

        protected override void OnButtonClick()
        {
            base.OnButtonClick();

            if (_gamePauser.IsPaused == false)
            {
                if (PlayerAccount.IsAuthorized == false)
                {
                    _loginPanel.gameObject.SetActive(true);
                }
                else
                {
                    AutorizationCompleted?.Invoke();
                }
            }
        }
    }
}