using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;
    [SerializeField] private TMP_Text _level;

    private void OnEnable()
    {
        _levelController.LevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _levelController.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(int levelNumber)
    {
        string levelText = "Level";
        _level.text = $"{levelText} {levelNumber}";
    }
}
