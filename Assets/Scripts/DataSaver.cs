using UnityEngine;
using UnityEngine.Events;

public class DataSaver : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;

    private const string _levelNumberKey = "Level";
    private int _playerPrefsSavedLevelNumber = 0;

    public event UnityAction<int> LevelNumberLoaded;

    private void Awake()
    {
        _playerPrefsSavedLevelNumber = PlayerPrefs.GetInt(_levelNumberKey, 0);
    }

    private void OnEnable()
    {
        _levelController.LevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _levelController.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(int level)
    {
        if (level != _playerPrefsSavedLevelNumber)
            CheckLevel(level);
    }

    private void CheckLevel(int level)
    {
        if (_playerPrefsSavedLevelNumber > level)
            LevelNumberLoaded?.Invoke(_playerPrefsSavedLevelNumber);
        else
        {
            _playerPrefsSavedLevelNumber = level;
            PlayerPrefs.SetInt(_levelNumberKey, _playerPrefsSavedLevelNumber);
            PlayerPrefs.Save();
        }    
    }
}
