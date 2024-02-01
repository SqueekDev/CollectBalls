using System;
using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Controller
{
    public class FieldsChanger : MonoBehaviour
    {
        [SerializeField] private LevelChanger _levelChanger;
        [SerializeField] private List<Field> _fields;

        private int _currentFieldIndex = 0;
        private Field _currentField;

        public event Action<int> Changed;
        public event Action<int> FieldSizeChanged;

        private void OnEnable()
        {
            _levelChanger.Changing += OnLevelChanging;
        }

        private void OnDisable()
        {
            _levelChanger.Changing -= OnLevelChanging;
        }

        private void ChangeField(int fieldIndex)
        {
            if (_currentField != null)
            {
                Destroy(_currentField.gameObject);
            }

            _currentField = Instantiate(_fields[fieldIndex], transform.position + _fields[fieldIndex].transform.localPosition, Quaternion.identity, transform);
            FieldSizeChanged?.Invoke(_currentField.FieldSizeModifier);
            Changed?.Invoke(_currentField.BallsCount);
        }

        private void OnLevelChanging(int levelNumber)
        {
            if (_fields != null)
            {
                if (levelNumber > _fields.Count)
                {
                    _currentFieldIndex = (levelNumber - _fields.Count - 1);
                }
                else
                {
                    _currentFieldIndex = levelNumber - 1;
                }

                ChangeField(_currentFieldIndex);
            }
        }
    }
}