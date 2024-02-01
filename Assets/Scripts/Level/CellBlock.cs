using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Rigidbody))]
    public class CellBlock : MonoBehaviour
    {
        private const float Range = 0.2f;

        [SerializeField] private List<Ball> _balls;
        [SerializeField] private Rigidbody _rigidbody;

        private bool _isReleased;

        public event Action Released;

        public int BallsCount => _balls.Count;

        private void Awake()
        {
            _isReleased = false;

            for (int i = 0; i < _balls.Count; i++)
            {
                _balls[i].Init(this);
            }
        }

        private void FixedUpdate()
        {
            if (_isReleased == false && _rigidbody != null)
            {
                RaycastHit[] hits = _rigidbody.SweepTestAll(Vector3.back, Range);

                if (hits.Length == 0)
                {
                    Released?.Invoke();
                    _isReleased = true;
                }
            }
        }
    }
}