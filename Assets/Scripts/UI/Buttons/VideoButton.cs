using Lean.Localization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class VideoButton : GameButton
    {
        private const string AddedSeconds = "+20";

        [SerializeField] private TMP_Text _view;
        [SerializeField] private LeanPhrase _phrase;

        private string _text;

        protected override void OnEnable()
        {
            base.OnEnable();
            _text = LeanLocalization.GetTranslationText(_phrase.name);
            string text = $"{AddedSeconds} {_text}";
            _view.text = text;
        }
    }
}