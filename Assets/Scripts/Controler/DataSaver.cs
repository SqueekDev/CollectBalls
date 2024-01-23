using System;
using Global;
using UnityEngine;

namespace Controller
{
    public class DataSaver : MonoBehaviour
    {
        [SerializeField] private LevelChanger _levelChanger;

        private int _playerPrefsSavedLevelNumber = 0;

        public event Action<int> LevelNumberLoaded;

        private void Awake()
        {
            _playerPrefsSavedLevelNumber = PlayerPrefs.GetInt(GlobalValues.PlayerPrefsLevelKey, GlobalValues.Zero);
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
                PlayerPrefs.SetInt(GlobalValues.PlayerPrefsLevelKey, _playerPrefsSavedLevelNumber);
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