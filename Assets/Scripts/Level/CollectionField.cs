using System;
using System.Collections.Generic;
using Controller;
using UnityEngine;

namespace Level
{
    public class CollectionField : MonoBehaviour
    {
        private const int NumberToPlaySound = 1;
        private const float MinScale = 0.3f;
        private const float MaxScale = 1f;

        [SerializeField] private FieldsChanger _fieldChanger;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        private float _volumeScale;
        private int _soundCounter = 0;
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
                PlaySound();

                if (_currentFieldBallsCount > 0 && _collectedBalls.Count >= _currentFieldBallsCount)
                {
                    AllBallsCollected?.Invoke();
                }
            }
        }

        private void PlaySound()
        {
            if (_soundCounter > NumberToPlaySound)
            {
                _volumeScale = UnityEngine.Random.Range(MinScale, MaxScale);
                _audioSource.PlayOneShot(_audioClip, _volumeScale);
                _soundCounter = 0;
            }
            else
            {
                _soundCounter++;
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