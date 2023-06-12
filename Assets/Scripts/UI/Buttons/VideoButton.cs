using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Lean.Localization;

public class VideoButton : GameButton
{
    private const string AddedSeconds = "+20";

    [SerializeField] private TMP_Text _view;
    [SerializeField] private LeanPhrase _phrase;

    private string _text;

    public event UnityAction Clicked;

    private void OnEnable()
    {
        Button.onClick.AddListener(OnButtonClick);
        _text = LeanLocalization.GetTranslationText(_phrase.name);
        _view.text = $"{AddedSeconds} {_text}";
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        Clicked?.Invoke();
    }
}
