using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GamePanel _loginPanel;
    [SerializeField] private GamePanel _lostPanel;
    [SerializeField] private LeaderboardView _leaderboardPanel;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _fihishPanel;
    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartLevelButton;
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private Button _nextLevelButton;
    [Header("Others")]
    [SerializeField] private DataSaver _dataController;
    [SerializeField] private CollectionField _collectionField;
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _lostSound;
    [SerializeField] private Timer _timer;
    [SerializeField] private List<Field> _fields;

    private int _currentFieldIndex = 0;
    private int _currentLevelNumber = 1;
    private Field _currentField;
    private Coroutine _fihishLevelCorutine;

    public event UnityAction<int> FieldChanged;
    public event UnityAction<int> LevelChanged;
    public event UnityAction LevelFinished;
    public event UnityAction LevelRestarted;

    public bool IsPaused { get; private set; }

    private void OnEnable()
    {
        _dataController.LevelNumberLoaded += OnLevelNumberLoaded;
        _collectionField.AllBallsCollected += OnAllBallsCollected;
        _startButton.onClick.AddListener(StartLevel);
        _restartLevelButton.onClick.AddListener(OnRestartLevelButtonClick);
        _nextLevelButton.onClick.AddListener(NextLevel);
        _restartButton.Clicked += RestartLevel;
        _loginPanel.Opened += OnPanelOpened;
        _loginPanel.Closed += OnPanelClosed;
        _lostPanel.Opened += OnPanelOpened;
        _lostPanel.Closed += OnPanelClosed;
        _leaderboardPanel.Opened += OnPanelOpened;
        _leaderboardPanel.Closed += OnPanelClosed;
        _timer.Expired += OnTimeExpired;
        _timer.Added += OnTimeAdded;
    }

    private void Start()
    {
        _startPanel.SetActive(true);
        IsPaused = true;

        if (_fields != null)
            ChangeLevel();
    }

    private void OnDisable()
    {
        _dataController.LevelNumberLoaded -= OnLevelNumberLoaded;
        _collectionField.AllBallsCollected -= OnAllBallsCollected;
        _startButton.onClick.RemoveListener(StartLevel);
        _nextLevelButton.onClick.RemoveListener(NextLevel);
        _restartLevelButton.onClick.RemoveListener(OnRestartLevelButtonClick);
        _restartButton.Clicked -= RestartLevel;
        _loginPanel.Opened -= OnPanelOpened;
        _loginPanel.Closed -= OnPanelClosed;
        _lostPanel.Opened -= OnPanelOpened;
        _lostPanel.Closed -= OnPanelClosed;
        _leaderboardPanel.Opened -= OnPanelOpened;
        _leaderboardPanel.Closed -= OnPanelClosed;
        _timer.Expired -= OnTimeExpired;
        _timer.Added -= OnTimeAdded;
    }

    private void OnTimeExpired()
    {
        _lostPanel.gameObject.SetActive(true);
        _lostSound.Play();
    }

    private void OnTimeAdded()
    {
        _lostPanel.gameObject.SetActive(false);
    }

    private void OnLevelNumberLoaded(int level)
    {
        _currentLevelNumber = level;
        int levelToIndexModifier = 1;

        if (level > _fields.Count)
            _currentFieldIndex = (level - _fields.Count - levelToIndexModifier);
        else
            _currentFieldIndex = level - levelToIndexModifier;

        ChangeLevel();
    }

    private void OnAllBallsCollected()
    {
        if (_fihishLevelCorutine != null)
            StopCoroutine(_fihishLevelCorutine);

        _fihishLevelCorutine = StartCoroutine(FinishLevel());
    }

    private IEnumerator FinishLevel()
    {
        float delay = 1f;
        IsPaused = true;
        yield return new WaitForSeconds(delay);
        LevelFinished?.Invoke();
        _winSound.Play();
        _fihishPanel.SetActive(true);
        _fihishLevelCorutine = null;
    }

    private void StartLevel()
    {
        _clickSound.Play();
        _startPanel.SetActive(false);
        IsPaused = false;
        int turorialLevel = 1;

        if (_currentLevelNumber == turorialLevel)
            _tutorial.gameObject.SetActive(true);
        else
            _tutorial.gameObject.SetActive(false);
    }

    private void OnRestartLevelButtonClick()
    {
        if (IsPaused == false)
            RestartLevel();
    }

    private void RestartLevel()
    {
        _clickSound.Play();
        ChangeField(_currentFieldIndex);
        LevelFinished?.Invoke();
        LevelRestarted?.Invoke();
    }

    private void NextLevel()
    {
        _clickSound.Play();
        _currentFieldIndex++;
        _currentLevelNumber++;

        if (_currentFieldIndex >= _fields.Count)
            _currentFieldIndex = 0;

        _fihishPanel.SetActive(false);
        IsPaused = false;
        ChangeLevel();
    }

    private void ChangeLevel()
    {
        ChangeField(_currentFieldIndex);
        LevelChanged?.Invoke(_currentLevelNumber);
    }

    private void ChangeField(int fieldIndex)
    {
        if (_currentField != null)
            Destroy(_currentField.gameObject);

        _currentField = Instantiate(_fields[fieldIndex], transform.position + _fields[fieldIndex].transform.localPosition, Quaternion.identity, transform);
        FieldChanged?.Invoke(_currentField.BallsCount);
    }

    private void OnPanelOpened()
    {
        IsPaused = true;
    }

    private void OnPanelClosed()
    {
        IsPaused = false;
    }
}
