using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CellBlock : MonoBehaviour
{
    [SerializeField] private List<Ball> _balls;
    [SerializeField]private Rigidbody _rigidbody;

    private float _range = 0.2f;
    private bool _isReleased;

    public event UnityAction Released;

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
            RaycastHit[] hits = _rigidbody.SweepTestAll(Vector3.back, _range);

            if (hits.Length == 0)
            {
                Released?.Invoke();
                _isReleased = true;
            }

        }
    }
}
