using System;
using System.Collections.Generic;
using Controller;
using UnityEngine;

namespace Level
{
    public class CollectionField : MonoBehaviour
    {
        [SerializeField] private FieldsChanger _fieldChanger;

        private int _currentFieldBallsCount;
        private List<Ball> _collectedBalls = new List<Ball>();

        public event Action<int> BallCollected;

        public event Action AllBallsCollected;

        private void OnEnable()
        {
            _fieldChanger.Changed += OnFieldChanged;
        }

        private void OnDisable()
        {
            _fieldChanger.Changed -= OnFieldChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                _collectedBalls.Add(ball);
                BallCollected?.Invoke(_collectedBalls.Count);

                if (_currentFieldBallsCount > 0
                    && _collectedBalls.Count >= _currentFieldBallsCount)
                {
                    AllBallsCollected?.Invoke();
                }
            }
        }

        private void OnFieldChanged(int ballsCount)
        {
            _currentFieldBallsCount = ballsCount;

            for (int i = 0; i < _collectedBalls.Count; i++)
            {
                Destroy(_collectedBalls[i].gameObject);
            }

            _collectedBalls.Clear();
        }
    }
}