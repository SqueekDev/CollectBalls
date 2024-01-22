using Controller;
using Lean.Localization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelText : MonoBehaviour
    {
        [SerializeField] private LevelChanger _levelChanger;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private LeanPhrase _phrase;

        private string _text;

        private void OnEnable()
        {
            _levelChanger.Changed += OnLevelChanged;
        }

        private void Start()
        {
            _text = LeanLocalization.GetTranslationText(_phrase.name);
        }

        private void OnDisable()
        {
            _levelChanger.Changed -= OnLevelChanged;
        }

        private void OnLevelChanged(int levelNumber)
        {
            string text = $"{_text} {levelNumber}";
            _level.text = text;
        }
    }
}