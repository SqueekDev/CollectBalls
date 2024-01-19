using Controller;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private CollectionField _collectionField;
        [SerializeField] private LevelChanger _levelChanger;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Slider _slider;

        private float _currentBallsCount;
        private float _maxBallsCount;

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

        private void ChangeText()
        {
            string text = $"{_currentBallsCount}/{_maxBallsCount}";
            _text.text = text;
        }

        private void ChangeSliderValue()
        {
            float sliderValue = _currentBallsCount / _maxBallsCount;
            _slider.value = sliderValue;
        }

        private void OnBallCollected(int currentBallsCount)
        {
            _currentBallsCount = currentBallsCount;

            if (_currentBallsCount > _maxBallsCount)
            {
                _currentBallsCount = _maxBallsCount;
            }

            ChangeSliderValue();
            ChangeText();
        }

        private void OnFieldChanged(int maxBallsCount)
        {
            _currentBallsCount = 0;
            _maxBallsCount = maxBallsCount;
            ChangeSliderValue();
            ChangeText();
        }
    }
}