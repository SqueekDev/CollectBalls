using System;

namespace UI
{
    public class LoginAcceptButton : GameButton
    {
        public event Action ButtonClicked;

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            ButtonClicked?.Invoke();
        }
    }
}