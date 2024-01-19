using Lean.Localization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LeaderboardPlayerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private LeanPhrase _phrase;

        private string _levelText;

        private void OnEnable()
        {
            _levelText = LeanLocalization.GetTranslationText(_phrase.name);
        }

        public void Init(string number, string name, string level)
        {
            _number.text = number;
            _name.text = name;
            string text = $"{_levelText} {level}";
            _level.text = text;
        }
    }
}