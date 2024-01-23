using System.Collections;
using UnityEngine;

namespace Level
{
    public class BoxShaker : MonoBehaviour
    {
        private const float MoveDelayDime = 0.01f;
        private const float ShakeRange = -0.1f;
        private const float Step = 0.2f;

        private float _moveProgress;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private Coroutine _shakeCorutine;
        private Coroutine _moveCorutine;
        private WaitForSeconds _moveDelay = new WaitForSeconds(MoveDelayDime);
        private WaitUntil _untilDelay;

        private void Awake()
        {
            _untilDelay = new WaitUntil(() => _moveCorutine == null);
            _startPosition = transform.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                CheckCorutine(_shakeCorutine);
                _targetPosition = new Vector3(_startPosition.x, _startPosition.y + ShakeRange, _startPosition.z);
                _shakeCorutine = StartCoroutine(Shake(_targetPosition));
            }
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
            while (transform.localPosition != targetPosition)
            {
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, _moveProgress);
                _moveProgress += Step;
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
    }
}