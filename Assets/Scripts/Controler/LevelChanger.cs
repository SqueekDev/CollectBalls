using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using UI;

namespace Controller
{
    public class LevelChanger : MonoBehaviour
    {
        private const float FinishLevelDelay = 1f;

        [Header("Buttons")]
        [SerializeField] private GameButton _restartLevelButton;
        [SerializeField] private RestartButton _restartButton;
        [SerializeField] private GameButton _nextLevelButton;
        [Header("Others")]
        [SerializeField] private DataSaver _dataSaver;
        [SerializeField] private GamePauser _gamePauser;
        [SerializeField] private CollectionField _collectionField;
        [SerializeField] private List<Field> _fields;

        private int _currentFieldIndex = 0;
        private int _currentLevelNumber = 1;
        private Field _currentField;
        private Coroutine _fihishLevelCorutine;
        private WaitForSeconds _delay = new WaitForSeconds(FinishLevelDelay);

        public event Action<int> FieldChanged;

        public event Action<int> FieldSizeChanged;

        public event Action<int> LevelChanged;

        public event Action LevelFinished;

        public event Action LevelRestarted;

        public int CurrentLevelNumber => _currentLevelNumber;

        private void OnEnable()
        {
            _dataSaver.LevelNumberLoaded += OnLevelNumberLoaded;
            _collectionField.AllBallsCollected += OnAllBallsCollected;
            _restartLevelButton.Clicked += OnRestartLevelButtonClicked;
            _nextLevelButton.Clicked += OnNextLevelButtonClicked;
            _restartButton.Clicked += RestartLevel;
        }

        private void Start()
        {
            if (_fields != null)
            {
                ChangeLevel();
            }
        }

        private void OnDisable()
        {
            _dataSaver.LevelNumberLoaded -= OnLevelNumberLoaded;
            _collectionField.AllBallsCollected -= OnAllBallsCollected;
            _nextLevelButton.Clicked -= OnNextLevelButtonClicked;
            _restartLevelButton.Clicked -= OnRestartLevelButtonClicked;
            _restartButton.Clicked -= RestartLevel;
        }

        private IEnumerator FinishLevel()
        {
            yield return _delay;
            LevelFinished?.Invoke();
            _fihishLevelCorutine = null;
        }

        private void RestartLevel()
        {
            ChangeField(_currentFieldIndex);
            LevelFinished?.Invoke();
            LevelRestarted?.Invoke();
        }

        private void ChangeLevel()
        {
            ChangeField(_currentFieldIndex);
            LevelChanged?.Invoke(_currentLevelNumber);
        }

        private void ChangeField(int fieldIndex)
        {
            if (_currentField != null)
            {
                Destroy(_currentField.gameObject);
            }

            _currentField = Instantiate(_fields[fieldIndex], transform.position + _fields[fieldIndex].transform.localPosition, Quaternion.identity, transform);
            FieldSizeChanged?.Invoke(_currentField.FieldSizeModifier);
            FieldChanged?.Invoke(_currentField.BallsCount);
        }

        private void OnRestartLevelButtonClicked()
        {
            if (_gamePauser.IsPaused == false)
            {
                RestartLevel();
            }
        }

        private void OnNextLevelButtonClicked()
        {
            _currentFieldIndex++;
            _currentLevelNumber++;

            if (_currentFieldIndex >= _fields.Count)
            {
                _currentFieldIndex = 0;
            }

            ChangeLevel();
        }

        private void OnLevelNumberLoaded(int level)
        {
            _currentLevelNumber = level;
            int levelToIndexModifier = 1;

            if (level > _fields.Count)
            {
                _currentFieldIndex = (level - _fields.Count - levelToIndexModifier);
            }
            else
            {
                _currentFieldIndex = level - levelToIndexModifier;
            }

            ChangeLevel();
        }

        private void OnAllBallsCollected()
        {
            if (_fihishLevelCorutine != null)
            {
                StopCoroutine(_fihishLevelCorutine);
            }

            _fihishLevelCorutine = StartCoroutine(FinishLevel());
        }
    }
}