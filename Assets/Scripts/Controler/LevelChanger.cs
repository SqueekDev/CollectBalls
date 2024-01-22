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
        [SerializeField] private FieldsChanger _fieldsChanger;

        private int _currentLevelNumber = 1;
        private Coroutine _fihishLevelCorutine;
        private WaitForSeconds _delay = new WaitForSeconds(FinishLevelDelay);

        public event Action<int> Changing;

        public event Action<int> Changed;

        public event Action Finished;

        public int CurrentLevelNumber => _currentLevelNumber;

        private void OnEnable()
        {
            _dataSaver.LevelNumberLoaded += OnLevelNumberLoaded;
            _collectionField.AllBallsCollected += OnAllBallsCollected;
            _fieldsChanger.Changed += OnFieldChanged;
            _restartLevelButton.Clicked += OnRestartLevelButtonClicked;
            _nextLevelButton.Clicked += OnNextLevelButtonClicked;
            _restartButton.Clicked += ChangeLevel;
        }

        private void Start()
        {
            ChangeLevel();
        }

        private void OnDisable()
        {
            _dataSaver.LevelNumberLoaded -= OnLevelNumberLoaded;
            _collectionField.AllBallsCollected -= OnAllBallsCollected;
            _fieldsChanger.Changed -= OnFieldChanged;
            _nextLevelButton.Clicked -= OnNextLevelButtonClicked;
            _restartLevelButton.Clicked -= OnRestartLevelButtonClicked;
            _restartButton.Clicked -= ChangeLevel;
        }

        private IEnumerator FinishLevel()
        {
            yield return _delay;
            Finished?.Invoke();
            _fihishLevelCorutine = null;
        }

        private void ChangeLevel()
        {
            Changing?.Invoke(_currentLevelNumber);
        }

        private void OnFieldChanged(int ballsCount)
        {
            Changed?.Invoke(_currentLevelNumber);
        }

        private void OnRestartLevelButtonClicked()
        {
            if (_gamePauser.IsPaused == false)
            {
                ChangeLevel();
            }
        }

        private void OnNextLevelButtonClicked()
        {
            _currentLevelNumber++;
            ChangeLevel();
        }

        private void OnLevelNumberLoaded(int level)
        {
            _currentLevelNumber = level;
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