using UnityEngine;
using TMPro;

public class TimerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _view;
    [SerializeField] private Timer _timer;
    [SerializeField] private Color _startTimeColor;
    [SerializeField] private Color _changedTimeColor;

    private const int _secondsInMinute = 60;
    private const float _timeToChangeColor = 6f;
    private int _seconds;
    private int _minutes;

    private void Update()
    {
        _minutes = (int)_timer.CurrentTime / _secondsInMinute;
        _seconds = (int)_timer.CurrentTime - _minutes * _secondsInMinute;
        _view.text = _seconds.ToString().Length == 1 ? $"{_minutes} : 0{_seconds}" : $"{_minutes} : {_seconds}";

        if (_timer.CurrentTime > _timeToChangeColor)
            _view.color = _startTimeColor;
        else
            _view.color = _changedTimeColor;
    }
}
