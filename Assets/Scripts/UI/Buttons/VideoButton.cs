using System;
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

        public event Action Clicked;

        private void OnEnable()
        {
            Button.onClick.AddListener(OnButtonClick);
            _text = LeanLocalization.GetTranslationText(_phrase.name);
            string text = $"{AddedSeconds} {_text}";
            _view.text = text;
        }

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            Clicked?.Invoke();
        }
    }
}