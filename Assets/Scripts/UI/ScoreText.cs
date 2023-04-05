using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private CollectionField _collectionField;
    [SerializeField] private LevelController _levelController;
    [SerializeField] private TMP_Text _score;

    private int _currentBallsCount;
    private int _maxBallsCount;

    private void OnEnable()
    {
        _collectionField.BallCollected += OnBallCollected;
        _levelController.FieldChanged += OnFieldChanged;
    }

    private void OnDisable()
    {
        _collectionField.BallCollected -= OnBallCollected;
        _levelController.FieldChanged -= OnFieldChanged;
    }

    private void OnBallCollected(int currentBallsCount)
    {
        _currentBallsCount = currentBallsCount;
        ChangeText();
    }

    private void OnFieldChanged(int maxBallsCount)
    {
        _currentBallsCount = 0;
        _maxBallsCount = maxBallsCount;
        ChangeText();
    }

    private void ChangeText()
    {
        _score.text = $"{_currentBallsCount}/{_maxBallsCount}";
    }
}
