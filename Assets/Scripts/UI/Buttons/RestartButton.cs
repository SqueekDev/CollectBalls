using UnityEngine;

namespace UI
{
    public class RestartButton : GameButton
    {
        [SerializeField] private GamePanel _panel;

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            _panel.gameObject.SetActive(false);
        }
    }
}