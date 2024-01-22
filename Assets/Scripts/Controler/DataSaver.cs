using System;
using UnityEngine;

namespace Controller
{
    public class DataSaver : MonoBehaviour
    {
        private const string LevelNumberKey = "Level";

        [SerializeField] private LevelChanger _levelChanger;

        private int _playerPrefsSavedLevelNumber = 0;

        public event Action<int> LevelNumberLoaded;

        public string KeyName => LevelNumberKey;

        private void Awake()
        {
            _playerPrefsSavedLevelNumber = PlayerPrefs.GetInt(LevelNumberKey, 0);
        }

        private void OnEnable()
        {
            _levelChanger.Changed += OnLevelChanged;
        }

        private void OnDisable()
        {
            _levelChanger.Changed -= OnLevelChanged;
        }

        private void CheckLevel(int level)
        {
            if (_playerPrefsSavedLevelNumber > level)
            {
                LevelNumberLoaded?.Invoke(_playerPrefsSavedLevelNumber);
            }
            else
            {
                _playerPrefsSavedLevelNumber = level;
                PlayerPrefs.SetInt(LevelNumberKey, _playerPrefsSavedLevelNumber);
                PlayerPrefs.Save();
            }
        }

        private void OnLevelChanged(int level)
        {
            if (level != _playerPrefsSavedLevelNumber)
            {
                CheckLevel(level);
            }
        }
    }
}