using UnityEngine;
using TMPro;
using Lean.Localization;

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
        _level.text = $"{_levelText} {level}";
    }
}
