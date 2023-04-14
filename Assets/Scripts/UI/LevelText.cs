using UnityEngine;
using TMPro;
using Lean.Localization;

public class LevelText : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private LeanPhrase _phrase; 

    private string _text;

    private void OnEnable()
    {
        _levelController.LevelChanged += OnLevelChanged;
    }

    private void Start()
    {
        _text = LeanLocalization.GetTranslationText(_phrase.name);
    }

    private void OnDisable()
    {
        _levelController.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(int levelNumber)
    {
        _level.text = $"{_text} {levelNumber}";
    }
}
