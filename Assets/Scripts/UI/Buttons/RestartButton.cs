using UnityEngine;
using UnityEngine.Events;

public class RestartButton : GameButton
{
    [SerializeField] private GamePanel _panel;

    public event UnityAction Clicked;

    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        Clicked?.Invoke();
        _panel.gameObject.SetActive(false);
    }
}
