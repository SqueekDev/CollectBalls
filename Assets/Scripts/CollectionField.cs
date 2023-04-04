using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectionField : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;

    private List<Ball> _collectedBalls = new List<Ball>();

    public float �ollectedBallsCount => _collectedBalls.Count;
    public float �urrentFieldBallsCount => _levelController.CurrentField.BallsCount;

    public event UnityAction BallCollected;
    public event UnityAction AllBallsCollected;

    private void OnEnable()
    {
        _levelController.FieldChanged += OnFieldChanged;
    }

    private void OnDisable()
    {
        _levelController.FieldChanged -= OnFieldChanged;
    }

    private void OnFieldChanged(float ballsCount)
    {
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
            BallCollected?.Invoke();

            if (�urrentFieldBallsCount > 0 && �ollectedBallsCount >= �urrentFieldBallsCount)
                AllBallsCollected?.Invoke();
        }
    }
}
