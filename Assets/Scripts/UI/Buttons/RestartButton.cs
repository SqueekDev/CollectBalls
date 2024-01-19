using System;
using UnityEngine;

namespace UI
{
    public class RestartButton : GameButton
    {
        [SerializeField] private GamePanel _panel;

        public event Action Clicked;

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            Clicked?.Invoke();
            _panel.gameObject.SetActive(false);
        }
    }
}