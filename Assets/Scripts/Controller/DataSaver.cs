using System;
using UnityEngine;

namespace Controller
{
    public class DataSaver : MonoBehaviour
    {
        private const string LevelKeyName = "Level";

        [SerializeField] private LevelChanger _levelChanger;

        private int _playerPrefsSavedLevelNumber = 0;

        public event Action<int> LevelNumberLoaded;

        public string KeyName => LevelKeyName;

        private void Awake()
        {
            _playerPrefsSavedLevelNumber = PlayerPrefs.GetInt(LevelKeyName, 0);
        }

        private void OnEnable()
        {
            _levelChanger.Changed += OnLevelChanged;
        }

        private void OnDisable()
        {
            _levelChanger.Changed -= OnLevelChanged;
        }

        private void UpdateSavedLevelNumber(int level)
        {
            if (_playerPrefsSavedLevelNumber > level)
            {
                LevelNumberLoaded?.Invoke(_playerPrefsSavedLevelNumber);
            }
            else
            {
                _playerPrefsSavedLevelNumber = level;
                PlayerPrefs.SetInt(LevelKeyName, _playerPrefsSavedLevelNumber);
                PlayerPrefs.Save();
            }
        }

        private void OnLevelChanged(int level)
        {
            if (level != _playerPrefsSavedLevelNumber)
            {
                UpdateSavedLevelNumber(level);
            }
        }
    }
}