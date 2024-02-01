using System;
using System.Collections;
using Level;
using UI;
using UnityEngine;

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
        [SerializeField] private GamePauseFlag _gamePauser;
        [SerializeField] private CollectionField _collectionField;
        [SerializeField] private FieldsChanger _fieldsChanger;

        private int _currentLevelNumber = 1;
        private Coroutine _fihishLevelCorutine;
        private WaitForSeconds _delay = new WaitForSeconds(FinishLevelDelay);

        public event Action<int> Changing;
        public event Action<int> Changed;
        public event Action Finished;

        private void OnEnable()
        {
            _dataSaver.LevelNumberLoaded += OnLevelNumberLoaded;
            _collectionField.AllBallsCollected += OnAllBallsCollected;
            _fieldsChanger.Changed += OnFieldChanged;
            _restartLevelButton.Clicked += OnRestartLevelButtonClicked;
            _nextLevelButton.Clicked += OnNextLevelButtonClicked;
            _restartButton.Clicked += OnRestartButtonClicked;
        }

        private void Start()
        {
            Changing?.Invoke(_currentLevelNumber);
        }

        private void OnDisable()
        {
            _dataSaver.LevelNumberLoaded -= OnLevelNumberLoaded;
            _collectionField.AllBallsCollected -= OnAllBallsCollected;
            _fieldsChanger.Changed -= OnFieldChanged;
            _nextLevelButton.Clicked -= OnNextLevelButtonClicked;
            _restartLevelButton.Clicked -= OnRestartLevelButtonClicked;
            _restartButton.Clicked -= OnRestartButtonClicked;
        }

        private IEnumerator FinishingLevel()
        {
            yield return _delay;
            Finished?.Invoke();
            _fihishLevelCorutine = null;
        }

        private void OnFieldChanged(int ballsCount)
        {
            Changed?.Invoke(_currentLevelNumber);
        }

        private void OnRestartButtonClicked()
        {
            Changing?.Invoke(_currentLevelNumber);
        }

        private void OnRestartLevelButtonClicked()
        {
            if (_gamePauser.IsPaused == false)
            {
                Changing?.Invoke(_currentLevelNumber);
            }
        }

        private void OnNextLevelButtonClicked()
        {
            _currentLevelNumber++;
            Changing?.Invoke(_currentLevelNumber);
        }

        private void OnLevelNumberLoaded(int level)
        {
            _currentLevelNumber = level;
            Changing?.Invoke(_currentLevelNumber);
        }

        private void OnAllBallsCollected()
        {
            if (_fihishLevelCorutine != null)
            {
                StopCoroutine(_fihishLevelCorutine);
            }

            _fihishLevelCorutine = StartCoroutine(FinishingLevel());
        }
    }
}