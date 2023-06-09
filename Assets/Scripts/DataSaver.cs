using UnityEngine;
using UnityEngine.Events;

public class DataSaver : MonoBehaviour
{
    private const string LevelNumberKey = "Level";

    [SerializeField] private LevelChanger _levelChanger;

    private int _playerPrefsSavedLevelNumber = 0;

    public event UnityAction<int> LevelNumberLoaded;

    private void Awake()
    {
        _playerPrefsSavedLevelNumber = PlayerPrefs.GetInt(LevelNumberKey, 0);
    }

    private void OnEnable()
    {
        _levelChanger.LevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _levelChanger.LevelChanged -= OnLevelChanged;
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
            PlayerPrefs.SetInt(LevelNumberKey, _playerPrefsSavedLevelNumber);
            PlayerPrefs.Save();
        }    
    }
}
