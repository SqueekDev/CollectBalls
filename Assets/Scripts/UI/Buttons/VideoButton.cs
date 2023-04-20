using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Lean.Localization;

public class VideoButton : GameButton
{
    [SerializeField] private TMP_Text _view;
    [SerializeField] private LeanPhrase _phrase;

    private const string _addedSeconds = "+20";
    private string _text;

    public event UnityAction Clicked;

    private void OnEnable()
    {
        Button.onClick.AddListener(OnButtonClick);
        _text = LeanLocalization.GetTranslationText(_phrase.name);
        _view.text = $"{_addedSeconds} {_text}";
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        Clicked?.Invoke();
    }
}
