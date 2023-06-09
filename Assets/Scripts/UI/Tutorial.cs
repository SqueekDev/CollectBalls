using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private CollectionField _collectionField;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Pointer _pointer;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _target;

    private Coroutine _moveCorutine;
    private float _step = 0.1f;
    private float _moveDelayTime = 0.05f;
    private float _cicleDelayTime = 0.5f;

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

    private void OnBallCollected(int count)
    {
        transform.gameObject.SetActive(false);
    }

    private IEnumerator MovePointer()
    {
        WaitForSeconds moveDelay = new WaitForSeconds(_moveDelayTime);
        WaitForSeconds cicleDelay = new WaitForSeconds(_cicleDelayTime);

        while (enabled)
        {
            float moveProgress = 0;
            _pointer.transform.localPosition = _start.localPosition;

            while (_pointer.transform.localPosition != _target.localPosition)
            {
                _pointer.transform.localPosition = Vector3.Lerp(_start.localPosition, _target.localPosition, moveProgress);
                moveProgress += _step;
                yield return moveDelay;
            }

            yield return cicleDelay;
        }        
    }

    private void CheckCorutine()
    {
        if (_moveCorutine != null)
            StopCoroutine(_moveCorutine);
    }
}
