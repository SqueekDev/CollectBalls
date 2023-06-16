using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private CollectionField _collectionField;
    [SerializeField] private LevelChanger _levelChanger;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Slider _slider;

    private int _currentBallsCount;
    private int _maxBallsCount;

    private void Awake()
    {
        _slider.value = 0;
    }

    private void OnEnable()
    {
        _collectionField.BallCollected += OnBallCollected;
        _levelChanger.FieldChanged += OnFieldChanged;
    }


    private void OnDisable()
    {
        _collectionField.BallCollected -= OnBallCollected;
        _levelChanger.FieldChanged -= OnFieldChanged;
    }

    private void Update()
    {
        _slider.value = (float)_currentBallsCount / _maxBallsCount;
    }

    private void OnBallCollected(int currentBallsCount)
    {
        _currentBallsCount = currentBallsCount;

        if (_currentBallsCount > _maxBallsCount)
            _currentBallsCount = _maxBallsCount;

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
        _text.text = $"{_currentBallsCount}/{_maxBallsCount}";
    }
}
