using UnityEngine;
using TMPro;
using Lean.Localization;

public class LevelText : MonoBehaviour
{
    [SerializeField] private LevelChanger _levelChanger;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private LeanPhrase _phrase; 

    private string _text;

    private void OnEnable()
    {
        _levelChanger.LevelChanged += OnLevelChanged;
    }

    private void Start()
    {
        _text = LeanLocalization.GetTranslationText(_phrase.name);
    }

    private void OnDisable()
    {
        _levelChanger.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(int levelNumber)
    {
        _level.text = $"{_text} {levelNumber}";
    }
}
