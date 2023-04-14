using UnityEngine;
using UnityEngine.Events;
using Agava.YandexGames;

public class DataController : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;

    private const string _levelNumberKey = "Level";
    private int _playerPrefsSavedLevelNumber = 0;
    private int _cloudSavedLevelNumber = 0;

    public event UnityAction<int> LevelNumberLoaded;

    private void Awake()
    {
        if (PlayerAccount.IsAuthorized)
            PlayerAccount.GetCloudSaveData((data) => int.TryParse(data, out _cloudSavedLevelNumber));
        
        _playerPrefsSavedLevelNumber = PlayerPrefs.GetInt(_levelNumberKey, 0);

        SyncData();
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
        if (level != _cloudSavedLevelNumber || level != _playerPrefsSavedLevelNumber)
            CheckLevel(level);
    }

    private void CheckLevel(int level)
    {
        if (PlayerAccount.IsAuthorized)
        {
            if (_cloudSavedLevelNumber > level)
                LevelNumberLoaded?.Invoke(_cloudSavedLevelNumber);
            else
            {
                _cloudSavedLevelNumber = level;
                PlayerAccount.SetCloudSaveData(_cloudSavedLevelNumber.ToString());
                SyncData();
            }
        }
        else
        {
            if (_playerPrefsSavedLevelNumber > level)
                LevelNumberLoaded?.Invoke(_playerPrefsSavedLevelNumber);
            else
            {
                SetInt(level);
                SyncData();
            }    
        }
    }

    private void SyncData()
    {
        if (_cloudSavedLevelNumber > _playerPrefsSavedLevelNumber)
            SetInt(_cloudSavedLevelNumber);
        else
        {
            _cloudSavedLevelNumber = _playerPrefsSavedLevelNumber;

            if (PlayerAccount.IsAuthorized)
                PlayerAccount.SetCloudSaveData(_cloudSavedLevelNumber.ToString());
        }
    }

    private void SetInt(int number)
    {
        _playerPrefsSavedLevelNumber = number;
        PlayerPrefs.SetInt(_levelNumberKey, _playerPrefsSavedLevelNumber);
        PlayerPrefs.Save();
    }
}
