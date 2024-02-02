using System.Collections;
using Controller;
using UnityEngine;

namespace Level
{
    public class FieldShaker : MonoBehaviour
    {
        private const float DelayTime = 0.01f;
        private const float Step = 0.2f;
        private const float RotationValue = 7f;

        private float _rotateProgress;
        private Quaternion _startRotation;
        private Coroutine _shakeCorutine;
        private Coroutine _rotateCorutine;
        private WaitUntil _delayUntil;
        private WaitForSeconds _delay = new WaitForSeconds(DelayTime);

        private void Awake()
        {
            _delayUntil = new WaitUntil(() => _rotateCorutine == null);
            _startRotation = transform.rotation;
        }

        private void OnEnable()
        {
            InputDetector.Swiped += OnSwiped;
        }

        private void OnDisable()
        {
            InputDetector.Swiped -= OnSwiped;
        }

        private IEnumerator Shake(Quaternion targetRotation)
        {
            transform.rotation = _startRotation;
            CheckCorutine(_rotateCorutine);
            _rotateCorutine = StartCoroutine(Rotate(_startRotation, targetRotation));
            yield return _delayUntil;
            CheckCorutine(_rotateCorutine);
            _rotateCorutine = StartCoroutine(Rotate(targetRotation, _startRotation));
            yield return _delayUntil;
            _shakeCorutine = null;
        }

        private IEnumerator Rotate(Quaternion startRotation, Quaternion targetRotation)
        {
            _rotateProgress = 0;

            while (transform.rotation != targetRotation)
            {
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, _rotateProgress);
                _rotateProgress += Step;
                yield return _delay;
            }

            _rotateCorutine = null;
        }

        private void CheckCorutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        private Quaternion GetRotation(Vector3 direction)
        {
            Quaternion rotation;

            if (direction.x != 0)
            {
                rotation = direction.x > 0
                    ? Quaternion.Euler(0, -RotationValue, 0)
                    : Quaternion.Euler(0, RotationValue, 0);
            }
            else
            {
                rotation = direction.y > 0
                    ? Quaternion.Euler(RotationValue, 0, 0)
                    : Quaternion.Euler(-RotationValue, 0, 0);
            }

            return rotation;
        }

        private void OnSwiped(Vector3 direction)
        {
            CheckCorutine(_shakeCorutine);
            Quaternion targetRotation = GetRotation(direction);
            _shakeCorutine = StartCoroutine(Shake(targetRotation));
        }
    }
}