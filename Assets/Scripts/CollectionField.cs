using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectionField : MonoBehaviour
{
    [SerializeField] private LevelChanger _levelChanger;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private int _soundCounter = 0;
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

    private void PlaySound()
    {
        int numberToPlaySound = 1;

        if (_soundCounter > numberToPlaySound)
        {
            float minScale = 0.3f;
            float maxScale = 1f;
            float volumeScale = Random.Range(minScale, maxScale);
            _audioSource.PlayOneShot(_audioClip, volumeScale);
            _soundCounter = 0;
        }
        else
        {
            _soundCounter++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            _collectedBalls.Add(ball);
            BallCollected?.Invoke(_collectedBalls.Count);
            PlaySound();

            if (_currentFieldBallsCount > 0 && _collectedBalls.Count >= _currentFieldBallsCount)
                AllBallsCollected?.Invoke();
        }
    }
}
