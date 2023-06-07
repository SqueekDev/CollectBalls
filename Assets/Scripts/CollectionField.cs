using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectionField : MonoBehaviour
{
    [SerializeField] private LevelChanger _levelChanger;

    private int _currentFieldBallsCount;
    private List<Ball> _collectedBalls = new List<Ball>();

    public event UnityAction<int> BallCollected;
    public event UnityAction AllBallsCollected;

    private void OnEnable()
    {
        _levelChanger.FieldChanged += OnFieldChanged;
    }

    private void OnDisable()
    {
        _levelChanger.FieldChanged -= OnFieldChanged;
    }

    private void OnFieldChanged(int ballsCount)
    {
        _currentFieldBallsCount = ballsCount;

        for (int i = 0; i < _collectedBalls.Count; i++)
            Destroy(_collectedBalls[i].gameObject);

        _collectedBalls.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            _collectedBalls.Add(ball);
            BallCollected?.Invoke(_collectedBalls.Count);

            if (_currentFieldBallsCount > 0 && _collectedBalls.Count >= _currentFieldBallsCount)
                AllBallsCollected?.Invoke();
        }
    }
}
