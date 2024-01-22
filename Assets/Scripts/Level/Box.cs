using System.Collections;
using Controller;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Animator))]
    public class Box : MonoBehaviour
    {
        private const string CloseTriggerName = "Close";
        private const string OpenTriggerName = "Open";
        private const float CloseDelayTime = 1f;
        private const float MoveDelayDime = 0.01f;

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
        private WaitForSeconds _closeDelay = new WaitForSeconds(CloseDelayTime);
        private WaitForSeconds _moveDelay = new WaitForSeconds(MoveDelayDime);
        private WaitUntil _untilDelay;

        private void Awake()
        {
            _untilDelay = new WaitUntil(() => _moveCorutine == null);
            _startPosition = transform.localPosition;
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _collectionField.AllBallsCollected += OnAllBallCollected;
            _levelChanger.Changed += OnLevelChanged;
        }

        private void OnDisable()
        {
            _collectionField.AllBallsCollected -= OnAllBallCollected;
            _levelChanger.Changed -= OnLevelChanged;
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
            yield return _closeDelay;
            _animator.SetTrigger(CloseTriggerName);
            _closeCorutine = null;
        }

        private IEnumerator Shake(Vector3 targetPosition)
        {
            transform.localPosition = _startPosition;
            CheckCorutine(_moveCorutine);
            _moveCorutine = StartCoroutine(Move(_startPosition, targetPosition));
            yield return _untilDelay;
            CheckCorutine(_moveCorutine);
            _moveCorutine = StartCoroutine(Move(targetPosition, _startPosition));
            yield return _untilDelay;
            _shakeCorutine = null;
        }

        private IEnumerator Move(Vector3 startPosition, Vector3 targetPosition)
        {
            float step = 0.2f;
            float moveProgress = 0;

            while (transform.localPosition != targetPosition)
            {
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, moveProgress);
                moveProgress += step;
                yield return _moveDelay;
            }

            _moveCorutine = null;
        }

        private void CheckCorutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        private void OnAllBallCollected()
        {
            CheckCorutine(_closeCorutine);
            _closeCorutine = StartCoroutine(Close());
        }

        private void OnLevelChanged(int levelNumber)
        {
            _animator.SetTrigger(OpenTriggerName);
        }
    }
}