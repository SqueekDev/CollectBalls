using System.Collections;
using Global;
using Level;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Tutorial : MonoBehaviour
    {
        private const float Step = 0.1f;
        private const float MoveDelayTime = 0.05f;
        private const float CicleDelayTime = 0.5f;

        [SerializeField] private CollectionField _collectionField;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Pointer _pointer;
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _target;

        private float _moveProgress;
        private Coroutine _moveCorutine;
        private WaitForSeconds _moveDelay = new WaitForSeconds(MoveDelayTime);
        private WaitForSeconds _cicleDelay = new WaitForSeconds(CicleDelayTime);

        private void OnEnable()
        {
            _leaderboardButton.interactable = false;
            _restartButton.interactable = false;
            _collectionField.BallCollected += OnBallCollected;
            CheckCorutine();
            _moveCorutine = StartCoroutine(MovePointer());
        }

        private void OnDisable()
        {
            _leaderboardButton.interactable = true;
            _restartButton.interactable = true;
            _collectionField.BallCollected -= OnBallCollected;
            CheckCorutine();
        }

        private IEnumerator MovePointer()
        {
            while (enabled)
            {
                _moveProgress = GlobalValues.Zero;
                _pointer.transform.localPosition = _start.localPosition;

                while (_pointer.transform.localPosition != _target.localPosition)
                {
                    _pointer.transform.localPosition = Vector3.Lerp(_start.localPosition, _target.localPosition, _moveProgress);
                    _moveProgress += Step;
                    yield return _moveDelay;
                }

                yield return _cicleDelay;
            }
        }

        private void CheckCorutine()
        {
            if (_moveCorutine != null)
            {
                StopCoroutine(_moveCorutine);
            }
        }

        private void OnBallCollected(int count)
        {
            transform.gameObject.SetActive(false);
        }
    }
}