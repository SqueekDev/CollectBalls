using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Box : MonoBehaviour
{
    private const string _closeTriggerName = "Close";
    private const string _openTriggerName = "Open";

    [SerializeField] private CollectionField _collectionField;
    [SerializeField] private LevelChanger _levelChanger;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private int _soundCounter = 0;
    private Animator _animator;
    private Vector3 _startPosition;
    private Coroutine _shakeCorutine;
    private Coroutine _moveCorutine;
    private Coroutine _closeCorutine;

    private void Awake()
    {
        _startPosition = transform.localPosition;
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _collectionField.AllBallsCollected += OnAllBallCollected;
        _levelChanger.LevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _collectionField.AllBallsCollected -= OnAllBallCollected;
        _levelChanger.LevelChanged -= OnLevelChanged;        
    }

    private void OnAllBallCollected()
    {
        CheckCorutine(_closeCorutine);
        _closeCorutine = StartCoroutine(Close());
    }

    private void OnLevelChanged(int levelNumber)
    {
        _animator.SetTrigger(_openTriggerName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            CheckCorutine(_shakeCorutine);
            float range = -0.1f;
            Vector3 targetPosition = new Vector3(_startPosition.x, _startPosition.y + range, _startPosition.z);
            _shakeCorutine = StartCoroutine(Shake(targetPosition));
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
    }

    private IEnumerator Close()
    {
        float delayTime = 1f;
        yield return new WaitForSeconds(delayTime);
        _animator.SetTrigger(_closeTriggerName);
        _closeCorutine = null;
    }

    private IEnumerator Shake(Vector3 targetPosition)
    {
        transform.localPosition = _startPosition;
        WaitUntil delay = new WaitUntil(() => _moveCorutine == null);
        CheckCorutine(_moveCorutine);
        _moveCorutine = StartCoroutine(Move(_startPosition, targetPosition));
        yield return delay;
        CheckCorutine(_moveCorutine);
        _moveCorutine = StartCoroutine(Move(targetPosition, _startPosition));
        yield return delay;
        _shakeCorutine = null;
    }

    private IEnumerator Move(Vector3 startPosition, Vector3 targetPosition)
    {
        float delayTime = 0.01f;
        WaitForSeconds delay = new WaitForSeconds(delayTime);
        float step = 0.2f;
        float moveProgress = 0;

        while (transform.localPosition != targetPosition)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, moveProgress);
            moveProgress += step;
            yield return delay;
        }

        _moveCorutine = null;
    }

    private void CheckCorutine(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
