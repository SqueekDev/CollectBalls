using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelChanger : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GamePanel _fihishPanel;
    [SerializeField] private Tutorial _tutorial;
    [Header("Buttons")]
    [SerializeField] private Button _restartLevelButton;
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private Button _nextLevelButton;
    [Header("Others")]
    [SerializeField] private DataSaver _dataSaver;
    [SerializeField] private GamePauser _gamePauser;
    [SerializeField] private CollectionField _collectionField;
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private List<Field> _fields;

    private int _currentFieldIndex = 0;
    private int _currentLevelNumber = 1;
    private Field _currentField;
    private Coroutine _fihishLevelCorutine;

    public event UnityAction<int> FieldChanged;
    public event UnityAction<int> LevelChanged;
    public event UnityAction<bool> PanelOpened;
    public event UnityAction LevelFinished;
    public event UnityAction LevelRestarted;

    private void OnEnable()
    {
        _dataSaver.LevelNumberLoaded += OnLevelNumberLoaded;
        _collectionField.AllBallsCollected += OnAllBallsCollected;
        _restartLevelButton.onClick.AddListener(OnRestartLevelButtonClick);
        _nextLevelButton.onClick.AddListener(NextLevel);
        _restartButton.Clicked += RestartLevel;
    }

    private void Start()
    {
        if (_fields != null)
            ChangeLevel();

        int turorialLevel = 1;

        if (_currentLevelNumber == turorialLevel)
            _tutorial.gameObject.SetActive(true);
        else
            _tutorial.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _dataSaver.LevelNumberLoaded -= OnLevelNumberLoaded;
        _collectionField.AllBallsCollected -= OnAllBallsCollected;
        _nextLevelButton.onClick.RemoveListener(NextLevel);
        _restartLevelButton.onClick.RemoveListener(OnRestartLevelButtonClick);
        _restartButton.Clicked -= RestartLevel;
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
        PanelOpened?.Invoke(true);
        yield return new WaitForSeconds(delay);
        LevelFinished?.Invoke();
        _winSound.Play();
        _fihishPanel.gameObject.SetActive(true);
        _fihishLevelCorutine = null;
    }

    private void OnRestartLevelButtonClick()
    {
        if (_gamePauser.IsPaused == false)
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

        _fihishPanel.gameObject.SetActive(false);
        PanelOpened?.Invoke(false);
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
}
