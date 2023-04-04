using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CollectionField _collectionField;
    [SerializeField] private List<Field> _fields;

    private int _currentFieldIndex = 0;
    private Coroutine _fihishLevelCorutine;

    public event UnityAction<float> FieldChanged; 

    public Field CurrentField { get; private set; }

    private void Awake()
    {
        if (_fields != null)
        {
            CurrentField = Instantiate(_fields[_currentFieldIndex], transform.position, Quaternion.identity, transform);
            FieldChanged?.Invoke(CurrentField.BallsCount);
        }
    }

    private void OnEnable()
    {
        _collectionField.AllBallsCollected += OnAllBallsCollected;
    }

    private void OnDisable()
    {
        _collectionField.AllBallsCollected -= OnAllBallsCollected;        
    }

    private void OnAllBallsCollected()
    {
        if (_fihishLevelCorutine != null)
            StopCoroutine(_fihishLevelCorutine);

        _fihishLevelCorutine = StartCoroutine(FinishLevel());
    }

    private IEnumerator FinishLevel()
    {
        float delay = 2f;
        yield return new WaitForSeconds(delay);
        ChangeField();
        FieldChanged?.Invoke(CurrentField.BallsCount);
        _fihishLevelCorutine = null;
    }

    private void ChangeField()
    {
        if (CurrentField != null)
            Destroy(CurrentField.gameObject);

        _currentFieldIndex++;

        if (_currentFieldIndex >= _fields.Count)
            _currentFieldIndex = 0;

        CurrentField = Instantiate(_fields[_currentFieldIndex], transform.position, Quaternion.identity, transform);
    }
}
