using UnityEngine;

namespace UI
{
    public class CloseButton : GameButton
    {
        [SerializeField] private GameObject _panel;

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            _panel.SetActive(false);
        }
    }
}