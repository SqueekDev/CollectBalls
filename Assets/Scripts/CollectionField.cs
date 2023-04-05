using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectionField : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;

    private int _currentFieldBallsCount;
    private List<Ball> _collectedBalls = new List<Ball>();

    private int _collectedBallsCount => _collectedBalls.Count;

    public event UnityAction<int> BallCollected;
    public event UnityAction AllBallsCollected;

    private void OnEnable()
    {
        _levelController.FieldChanged += OnFieldChanged;
    }

    private void OnDisable()
    {
        _levelController.FieldChanged -= OnFieldChanged;
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
            ball.transform.parent = transform;
            _collectedBalls.Add(ball);
            BallCollected?.Invoke(_collectedBallsCount);

            if (_currentFieldBallsCount > 0 && _collectedBallsCount >= _currentFieldBallsCount)
                AllBallsCollected?.Invoke();
        }
    }
}
