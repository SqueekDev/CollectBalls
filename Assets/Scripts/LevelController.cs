using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CollectionField _collectionField;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private GameObject _panel;
    [SerializeField] private List<Field> _fields;

    private int _currentFieldIndex = 0;
    private int _currentLevelNumber = 1;
    private Field _currentField;
    private Coroutine _fihishLevelCorutine;

    public event UnityAction<int> FieldChanged;
    public event UnityAction<int> LevelChanged;

    public bool IsPaused { get; private set; }

    private void OnEnable()
    {
        _collectionField.AllBallsCollected += OnAllBallsCollected;
        _restartButton.onClick.AddListener(RestartLevel);
        _nextLevelButton.onClick.AddListener(NextLevel);
    }

    private void Start()
    {
        _panel.gameObject.SetActive(false);

        if (_fields != null)
        {
            ChangeField(_currentFieldIndex);
            LevelChanged?.Invoke(_currentLevelNumber);
        }
    }

    private void OnDisable()
    {
        _collectionField.AllBallsCollected -= OnAllBallsCollected;
        _restartButton.onClick.RemoveListener(RestartLevel);
        _nextLevelButton.onClick.RemoveListener(NextLevel);
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
        _panel.gameObject.SetActive(true);
        _fihishLevelCorutine = null;
    }

    private void RestartLevel()
    {
        if (IsPaused == false)
            ChangeField(_currentFieldIndex);
    }

    private void NextLevel()
    {
        _currentFieldIndex++;
        _currentLevelNumber++;

        if (_currentFieldIndex >= _fields.Count)
            _currentFieldIndex = 0;

        ChangeField(_currentFieldIndex);
        LevelChanged?.Invoke(_currentLevelNumber);
        _panel.gameObject.SetActive(false);
        IsPaused = false;
    }

    private void ChangeField(int fieldIndex)
    {
        if (_currentField != null)
            Destroy(_currentField.gameObject);

        _currentField = Instantiate(_fields[fieldIndex], transform.position, Quaternion.identity, transform);
        FieldChanged?.Invoke(_currentField.BallsCount);
    }
}
