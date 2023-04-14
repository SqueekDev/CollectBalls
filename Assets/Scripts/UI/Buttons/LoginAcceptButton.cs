using UnityEngine.Events;

public class LoginAcceptButton : GameButton
{
    public event UnityAction ButtonClicked;

    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        ButtonClicked?.Invoke();
    }
}
